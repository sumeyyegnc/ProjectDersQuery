$(document).ready(function () {
    listeYukle();

    $("#btnYeniEkle").click(function () {
        formuTemizle();
        $("#kursModalBaslik").text("Yeni Kurs");
        $("#kursModal").modal("show");
    });

    $("#btnKaydet").click(function () {
        kaydet();
    });

    $("#tblKurs").on("click", ".btn-duzenle", function () {
        var id = $(this).data("id");
        duzenle(id);
    });

    $("#tblKurs").on("click", ".btn-sil", function () {
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
        url: "/Kurs/GetAll",
        type: "GET",
        data: { search: searchTerm },
        dataType: "json",
        success: function (data) {
            tabloyuDoldur(data);
        },
        error: function () {
            alert("Kurs listesi yüklenirken bir hata oluştu.");
        }
    });
}

function tabloyuDoldur(data) {
    if ($.fn.DataTable.isDataTable('#tblKurs')) {
        $('#tblKurs').DataTable().destroy();
    }

    var tbody = $("#tblKurs tbody");
    tbody.empty();

    $.each(data, function (i, item) {
        var satir = "<tr>" +
            "<td>" + item.KursId + "</td>" +
            "<td>" + item.KursAdi + "</td>" +
            "<td>" + item.Ucret + "</td>" +
            "<td>" +
            "<button type='button' class='btn btn-warning btn-sm btn-duzenle' data-id='" + item.KursId + "'>Düzenle</button> " +
            "<button type='button' class='btn btn-danger btn-sm btn-sil' data-id='" + item.KursId + "'>Sil</button>" +
            "</td>" +
            "</tr>";
        tbody.append(satir);
    });

    $('#tblKurs').DataTable({
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
    var id = parseInt($("#txtKursId").val());

    var model = {
        KursId: id,
        KursAdi: $("#txtKursAdi").val(),
        Ucret: parseInt($("#txtUcret").val())
    };

    var url = id === 0 ? "/Kurs/Create" : "/Kurs/Update";

    $.ajax({
        url: url,
        type: "POST",
        data: model,
        success: function (response) {
            if (response.success) {
                $("#kursModal").modal("hide");
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
        url: "/Kurs/GetById",
        type: "GET",
        data: { id: id },
        dataType: "json",
        success: function (item) {
            $("#txtKursId").val(item.KursId);
            $("#txtKursAdi").val(item.KursAdi);
            $("#txtUcret").val(item.Ucret);

            $("#kursModalBaslik").text("Kurs Düzenle");
            $("#kursModal").modal("show");
        },
        error: function () {
            alert("Kayıt getirilirken bir hata oluştu.");
        }
    });
}

function sil(id) {
    if (!confirm("Bu kursu silmek istediğinize emin misiniz?")) {
        return;
    }

    $.ajax({
        url: "/Kurs/Delete",
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
    $("#txtKursId").val(0);
    $("#txtKursAdi").val("");
    $("#txtUcret").val("");
}
