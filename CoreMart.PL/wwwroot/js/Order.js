
var Dtb;

$(document).ready(function () {
    LoadData();
});

function LoadData() {
    Dtb = $("#productTable").DataTable({
        "ajax": {
            "url": "/Admin/Order/GetData",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "id" },
            { "data": "fullName" },
            { "data": "phoneNumber" },
            { "data": "applicationUser.email" },
            { "data": "orderStatus" },
            { "data": "totalAmount" },


            {
                "data": "id",
                "render": function (data) {
                    return `
                                     <a href="/Admin/Order/Details?orderid=${data}" class="btn btn-warning">Details</a>                            `
                }
            }

        ]
    });
}



//function DeleteItem(url) {
//    Swal.fire({
//        title: "Are you sure?",
//        text: "You won't be able to revert this!",
//        icon: "warning",
//        showCancelButton: true,
//        confirmButtonColor: "#3085d6",
//        cancelButtonColor: "#d33",
//        confirmButtonText: "Yes, delete it!"
//    }).then((result) => {
//        if (result.isConfirmed) {
//            $.ajax({
//                url: url,
//                type: "Delete",
//                success: function (data) {
//                    if (data.success) {
//                        Dtb.ajax.reload();
//                        toastr.success(data.message);
//                    }
//                    else {
//                        toastr.error(data.message);
//                    }
//                }
//            });
//            Swal.fire({
//                title: "Deleted!",
//                text: "Your file has been deleted.",
//                icon: "success"
//            });
//        }
//    });
//}