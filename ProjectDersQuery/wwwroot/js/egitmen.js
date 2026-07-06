$(document).ready(function () {
    listeYukle();

    $("#btnYeniEkle").click(function () {
        formuTemizle();
        $("#egitmenModalBaslik").text("Yeni Eğitmen");
        $("#egitmenModal").modal("show");
    });

    $("#btnKaydet").click(function () {
        kaydet();
    });

    $("#tblEgitmen").on("click", ".btn-duzenle", function () {
        var id = $(this).data("id");
        duzenle(id);
    });

    $("#tblEgitmen").on("click", ".btn-sil", function () {
        var id = $(this).data("id");
        sil(id);
    });
    $("#btnSearch").click(function() {
        var searchTerm = $("#txtSearch").val();
        listeYukle(searchTerm);
    });
});

function listeYukle(searchTerm = "") {
    $.ajax({
        url: "/Egitmen/GetAll",
        type: "GET",
        data: { search: searchTerm },
        dataType: "json",
        success: function (data) {
            tabloyuDoldur(data);
        },
        error: function () {
            alert("Eğitmen listesi yüklenirken bir hata oluştu.");
        }
    });
}

function tabloyuDoldur(data) {
    if ($.fn.DataTable.isDataTable('#tblEgitmen')) {
        $('#tblEgitmen').DataTable().destroy();
    }

    var tbody = $("#tblEgitmen tbody");
    tbody.empty();

    $.each(data, function (i, item) {
        var satir = "<tr>" +
            "<td>" + item.EgitmenId + "</td>" +
            "<td>" + item.EgitmenAdi + "</td>" +
            "<td>" + item.Brans + "</td>" +
            "<td>" +
            "<button type='button' class='btn btn-warning btn-sm btn-duzenle' data-id='" + item.EgitmenId + "'>Düzenle</button> " +
            "<button type='button' class='btn btn-danger btn-sm btn-sil' data-id='" + item.EgitmenId + "'>Sil</button>" +
            "</td>" +
            "</tr>";
        tbody.append(satir);
    });

    $('#tblEgitmen').DataTable({
        dom: 'Brtip',
        buttons: [
            'excelHtml5',
            'pdfHtml5'
        ],
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.6/i18n/tr.json"
        }
    });
}

function kaydet() {
    var id = parseInt($("#txtEgitmenId").val());

    var model = {
        EgitmenId: id,
        EgitmenAdi: $("#txtEgitmenAdi").val(),
        Brans: $("#txtBrans").val()
    };

    var url = id === 0 ? "/Egitmen/Create" : "/Egitmen/Update";

    $.ajax({
        url: url,
        type: "POST",
        data: model,
        success: function (response) {
            if (response.success) {
                $("#egitmenModal").modal("hide");
                listeYukle();
            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Kayıt işlemi sırasında bir hata oluştu.");
        }
    });
}

function duzenle(id) {
    $.ajax({
        url: "/Egitmen/GetById",
        type: "GET",
        data: { id: id },
        dataType: "json",
        success: function (item) {
            $("#txtEgitmenId").val(item.EgitmenId);
            $("#txtEgitmenAdi").val(item.EgitmenAdi);
            $("#txtBrans").val(item.Brans);

            $("#egitmenModalBaslik").text("Eğitmen Düzenle");
            $("#egitmenModal").modal("show");
        },
        error: function () {
            alert("Kayıt getirilirken bir hata oluştu.");
        }
    });
}

function sil(id) {
    if (!confirm("Bu eğitmeni silmek istediğinize emin misiniz?")) {
        return;
    }

    $.ajax({
        url: "/Egitmen/Delete",
        type: "POST",
        data: { id: id },
        success: function (response) {
            if (response.success) {
                listeYukle();
            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("Silme işlemi sırasında bir hata oluştu.");
        }
    });
}

function formuTemizle() {
    $("#txtEgitmenId").val(0);
    $("#txtEgitmenAdi").val("");
    $("#txtBrans").val("");
}
