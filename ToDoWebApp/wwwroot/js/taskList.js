$(document).ready(function () {

    loadTabData("all");
    $(".nav-link").click(function () {
        var tab = $(this).data("tab");
        $(".nav-link").removeClass("active");
        $(this).addClass("active");
        loadTabData(tab);
    });
    function loadTabData(tab) {
        $.ajax({
            url: "/Task/GetTasks?tab=" + tab,
            type: "GET",
            success: function (data) {
                $("#taskTable tbody").html(data);
            }
        });
    }


    //Task actions
    $("#taskTable").on("change", ".completedCheckbox", function () {
        var $checkbox = $(this);
        var taskId = $checkbox.closest("tr").data("taskId");
        var completed = $checkbox.prop("checked");

        $.ajax({
            url: "/Task/CompleteTask",
            type: "POST",
            data: { id: taskId, completed: completed },
            success: function () {
                if (completed) {
                    $checkbox.closest("tr").addClass("completedTask");
                } else {
                    $checkbox.closest("tr").removeClass("completedTask");
                }
                reloadTab();
            }
        });
    });
    $("#taskTable").on("click", ".delete-task", function (e) {

        e.preventDefault(); 

        var $deleteBtn = $(this);
        var taskId = $deleteBtn.closest("tr").data("taskId");

        if (confirm('Are you sure you want to delete this task?')) {
            deleteTask(taskId);
        }
    });
    $("#taskTable") .on("click", ".edit-task", function (e) {
        e.preventDefault();

        var $editBtn = $(this);
        var taskId = $editBtn.closest("tr").data("taskId");
        var title = $(this).closest('tr').find('.task-name').text(); 
        var description = $(this).closest('tr').find('.task-description').text(); 
        var dueDate = $(this).closest('tr').find('.task-date').text();

        showEditTaskPop(taskId, title, description, dueDate);
    });


    $(".btn-add-item").hover(function () {
        $(this).removeClass("btn-outline-primary");
        $(this).addClass("btn-primary");
    });

    $(".btn-add-item").mouseleave(function () {
        $(this).addClass("btn-outline-primary");
        $(this).removeClass("btn-primary");
    });


    function showEditTaskPop(taskId, title, description, dueDate) {
        var modalHtml = `
    <div class="modal fade" id="editTaskModal" tabindex="-1" role="dialog" aria-labelledby="editTaskModalLabel" aria-hidden="true" data-bs-backdrop='static'>
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <h2 class="modal-title fw-bold mb-2 text-uppercase" id="editTaskModalLabel">Edit Task</h2>
                    <button type="button" class="close close-modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form id="editTaskForm">
                        <div class="form-group">
                            <label for="title">Task Title</label>
                            <input type="text" class="form-control" id="editTitle" name="title" required>
                        </div>
                        <div class="form-group">
                            <label for="description">Description</label>
                            <input type="text" class="form-control" id="editDescription" name="description" required>
                        </div>
                        <div class="form-group">
                            <label for="dueDate">Due Date</label>
                            <input type="date" class="form-control" id="editdueDate" name="dueDate" required>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary close-modal">Close</button>
                    <button type="submit" class="btn px-5 gradient-custom text-white" id="btnEditTask">Save Task</button>
                </div>
            </div>
        </div>
    </div>

    `;

        $('body').append(modalHtml);

        
        $('#editDescription').val(description);
        $('#editTitle').val(title);
        dueDate = formatDateforDatePicker(dueDate);
        $('#editdueDate').val(dueDate);
        

        $('#editTaskModal').modal('show');


        $('#btnEditTask').on('click', function () {

            var title = $('#editTitle').val();
            var description = $('#editDescription').val();
            var dueDate = $('#editdueDate').val();

            var isPast = isPastDate(dueDate);
            if (isPast) {
                return;
            }

            var isValid = validateFields(title, description, dueDate);
            if (!isValid) {
                return;
            }

            $.ajax({
                url: '/Task/UpdateTask',
                method: 'POST',
                data: { taskId: taskId, title: title, description: description},
                success: function (response) {
                    $('#editTaskModal').modal('hide');
                    $('#editTaskModal').remove();
                    reloadTab();
                },
                error: function (xhr, status, error) {
                    $('#editTaskModal').modal('hide');
                    $('#editTaskModal').remove();
                    showErrorPopup("Unable to Update ToDo Item.")
                }
            });
        });

        $(".close-modal").on('click', function () {
            $('#editTaskModal').modal('hide');
            $('#editTaskModal').remove();
        });
    }

    function showAddTaskPopup() {
        var modalHtml = `
    <div class="modal fade" id="addTaskModal" tabindex="-1" role="dialog" aria-labelledby="addTaskModalLabel" aria-hidden="true" data-bs-backdrop='static'>
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <h2 class="modal-title fw-bold mb-2 text-uppercase" id="addTaskModalLabel">Add New Task</h2>
                    <button type="button" class="close close-modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <form id="addTaskForm">
                        <div class="form-group">
                            <label for="title">Task Title</label>
                            <input type="text" class="form-control" id="title" name="title" required>
                        </div>
                        <div class="form-group">
                            <label for="description">Description</label>
                            <input type="text" class="form-control" id="description" name="description" required>
                        </div>
                        <div class="form-group">
                            <label for="dueDate">Due Date</label>
                            <input type="date" class="form-control" id="dueDate" name="dueDate" required>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary close-modal">Close</button>
                    <button type="submit" class="btn px-5 gradient-custom text-white" id="btnSaveTask">Save Task</button>
                </div>
            </div>
        </div>
    </div>

    `;

        $('body').append(modalHtml);
        $('#addTaskModal').modal('show');

        
        $('#btnSaveTask').on('click', function () {
            // Get task details from form fields
            var title = $('#title').val();
            var description = $('#description').val();
            var dueDate = $('#dueDate').val();

            var isPast = isPastDate(dueDate);
            if (isPast) {
                return;
            }

            var isValid = validateFields(title, description, dueDate);
            if (!isValid) {
                return;
            }

            
            $.ajax({
                url: '/Task/AddTask',
                method: 'POST',
                data: { title: title, description: description, dueDate: dueDate },
                success: function (response) {
                    $('#addTaskModal').modal('hide');
                    $('#addTaskModal').remove();
                    reloadTab();
                },
                error: function (xhr, status, error) {
                    $('#addTaskModal').modal('hide');
                    $('#addTaskModal').remove();
                    showErrorPopup("Unable to create ToDo Item.");

                }
            });
        });

        $(".close-modal").on('click', function () {
            $('#addTaskModal').modal('hide');
            $('#addTaskModal').remove();
        });
    }

    $('#btnAddTask').on('click', showAddTaskPopup);

    function deleteTask(taskId) {
        $.ajax({
            url: '/Task/DeleteTask', 
            method: 'POST',
            data: { taskID: taskId },
            success: function (response) {
                $('#tr_' + taskId).remove();
                reloadTab();
            },
            error: function (xhr, status, error) {
                showErrorPopup("Unable to Delete Task");
            }
        });
    }

    function reloadTab() {

        var activeNavTab = $('.nav-item .active').data("tab");
        loadTabData(activeNavTab);

    }

    function showErrorPopup(errorMsg) {
        var html =`<div id="alert-modal" class="modal fade data-bs-backdrop='static's">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header text-center">
                        <h4 class="modal-title">Error</h2>
                        <button type="button" class="close close-modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div id="alert-modal-body" class="modal-body"></div>
                </div>
            </div>
        </div>`

        $('body').append(html);
        $('#alert-modal-body').html(errorMsg);

        $('#alert-modal').modal('show');
        $(".close-modal").on('click', function () {
            $('#alert-modal').modal('hide');
            $('#alert-modal').remove();
        });
    }

    function validateFields(title, description, dueDate) {
        if (title == "" || description == "" || dueDate == "") {
            showErrorPopup("Please input all fields.");
            return false;
        }
        return true;
    }
    function isPastDate(selectedDate) {
        const today = new Date();
        const selected = new Date(selectedDate);

        if (selected < today) {
            showErrorPopup("Selected date cannot be in the past.");
            return true;
        }
        return false;
    }

    function formatDateforDatePicker(inputDate) {
        var parts = inputDate.split("/");

        var year = parts[2];
        var month = parts[0].padStart(2, '0'); 
        var day = parts[1].padStart(2, '0');

        var formattedDate = year + "-" + month + "-" + day;

        return formattedDate;
    }

});