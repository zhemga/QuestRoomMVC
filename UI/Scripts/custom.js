window.onload = loadCart();

function onSearch(e) {
    if (e.key === "Enter") {
        location.href = `/Rooms/Index?search=${e.target.value}`;
    }
}

function ChoosenUrlImg() {
    $("#urlimage").addClass("d-block");
    $("#localImg").addClass("d-none");
}

function ChoosenLocalImg() {
    $("#urlimage").addClass("d-none");
    $("#localImg").addClass("d-block");
}

function setFilter(element) {
    var type = $(element).data("type");
    var value = $(element).val();

    $("#roomsContainer").load(`/Rooms/Filter?type=${type}&value=${encodeURIComponent(value)}`);
}

function index() {
    window.location = `/Rooms/Index`;
}

function showRoomDetails(id) {
    window.location = `/Rooms/Room?id=${id}`;
}

function addRoom() {
    window.location = `/Rooms/AddRoom`;
}

function deleteRoom(id) {

    if (!confirm('Are you sure?')) {
        e.preventDefault();
    }
    window.location = `/Rooms/DeleteRoom?id=${id}`;
}

function editRoom(id) {
    window.location = `/Rooms/EditRoom?id=${id}`;
}

$(function () {

    $(document).on('change', '#selectImageFile', function () {
        var input = $(this),
            numFiles = input.get(0).files ? input.get(0).files.length : 1,
            label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
        if (numFiles == 1) {
            input.trigger('fileselect', label);
        }
        else {
            alert("You can choose only one image at once!")
        }

    });


    $(document).ready(function () {
        $('#selectImageFile').on('fileselect', function (event, label) {

            var input = $(this).parents('.input-group').find(':text');

            if (input.length) {
                input.val(label);
            } else {
                if (log) alert(label);
            }

        });
    });

});

function addUrlToList() {

    var imageUrl = $("#imageUrl").val();

    if (imageUrl.indexOf('http://') === 0 || imageUrl.indexOf('https://') === 0) {
        var listOfImages = $("#listOfImages");

        if (listOfImages.val() != "") {
            listOfImages.val(listOfImages.val() + ",");
        }

        listOfImages.val(listOfImages.val() + imageUrl);
    }
    else {
        alert("Wrong URL!");
    }
}

function addFileToList() {
    var formdata = new FormData();
    var fileInput = $("#selectImageFile");
    var btn = $("#btnAddFile");

    if (fileInput[0].files.length == 1) {
        btn.prop('disabled', true);

        formdata.append("img", fileInput.prop('files')[0]);

        var xhr = new XMLHttpRequest();
        xhr.open('POST', '/Rooms/UploadImage');
        xhr.send(formdata);
        xhr.onreadystatechange = function () {
            if (xhr.readyState == 4 && xhr.status == 200) {
                var listOfImages = $("#listOfImages");

                if (listOfImages.val() != "") {
                    listOfImages.val(listOfImages.val() + ',');
                }

                var respond = xhr.responseText;
                listOfImages.val(listOfImages.val() + respond.replace(/"/g, ""));
                btn.prop('disabled', false);
            }
        }
    }
    else {
        alert("Please, choose one file.")
    }
}

function clearList() {
    $("#listOfImages").val("");
}

function controlDecorations() {
    window.location = `/Rooms/ControlDecorations`;
}

function controlCompanies() {
    window.location = `/Rooms/ControlCompanies`;
}

function addOrder(id) {
    this.disabled = true;

    var orders = JSON.parse(localStorage.getItem("orders"));

    if (orders == null) {
        orders = [];
        localStorage.setItem("orders", JSON.stringify(orders));
    }

    if (orders.indexOf(id) < 0) {
        orders.push(id);
        localStorage.setItem("orders", JSON.stringify(orders));
        loadCart();
        alert("Order was added successfully!");
    }
    else
        alert("This order has already added!");


    this.disabled = false;
}

function loadCart() {
    var amountOfOrdersSpan = $("#amountOfOrders");

    var orders = JSON.parse(localStorage.getItem("orders"));
    if (orders == null)
        amountOfOrdersSpan.html(0);
    else
        amountOfOrdersSpan.html(orders.length);
}

function openCart() {
    var orders = JSON.parse(localStorage.getItem("orders"));
    if (orders == null)
        alert("Cart is empty!");
    else {
        window.location = `/Rooms/OpenCart?data=${JSON.stringify(orders)}`;
    }
}

function incrementValue(id) {
    const maxNumberOfHours = 24;
    var input = $("#" + id);
    var currentValue = input.val();

    if (!isNaN(currentValue) && currentValue < maxNumberOfHours) {
        input.val(parseInt(currentValue) + 1);
    } else {
        input.val(1);
    }
}

function decrementValue(id) {
    var input = $("#" + id);
    var currentValue = input.val();

    if (!isNaN(currentValue) && currentValue > 1) {
        input.val(parseInt(currentValue) - 1);
    } else {
        input.val(1);
    }
}