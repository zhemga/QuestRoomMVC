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

function addOrder(id) {
    window.location = `/Rooms/AddOrder?id=${id}`;
}

function deleteRoom(id) {

    if (!confirm('Are you sure?')) {
        e.preventDefault();
    }
    window.location = `/Rooms/Delete?id=${id}`;
}

function editRoom(id) {
    window.location = `/Rooms/Edit?id=${id}`;
}