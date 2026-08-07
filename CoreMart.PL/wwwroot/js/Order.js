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
            { "data": "id", "width": "6%" },
            { "data": "fullName" },
            { "data": "phoneNumber" },
            { "data": "applicationUser.email" },
            {
                "data": "orderStatus",
                "render": function (data) {
                    var badgeClass = "cm-badge-secondary";
                    switch (data) {
                        case "Approved": badgeClass = "cm-badge-info"; break;
                        case "Processing": badgeClass = "cm-badge-warning"; break;
                        case "Shipped": badgeClass = "cm-badge-primary"; break;
                        case "Cancelled": badgeClass = "cm-badge-danger"; break;
                        case "Refund": badgeClass = "cm-badge-secondary"; break;
                        default: badgeClass = "cm-badge-dark"; break;
                    }
                    return `<span class="cm-badge ${badgeClass}">${data}</span>`;
                }
            },
            {
                "data": "totalAmount",
                "render": function (data) {
                    return `<span class="fw-bold text-primary">$${parseFloat(data).toFixed(2)}</span>`;
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="text-end">
                        <a href="/Admin/Order/Details?orderid=${data}" class="btn btn-sm btn-cm-outline py-1 px-2">
                            <i class="bi bi-eye me-1"></i>Details
                        </a>
                    </div>`;
                }
            }
        ],
        "language": {
            "emptyTable": "No orders found",
            "search": "",
            "searchPlaceholder": "Search orders..."
        },
        "dom": '<"d-flex flex-wrap justify-content-between align-items-center mb-3"lf>rt<"d-flex flex-wrap justify-content-between align-items-center mt-3"ip>'
    });
}