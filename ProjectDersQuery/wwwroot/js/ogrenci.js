$(document).ready(function () {
    listeYukle();

    // Yeni ekle butonu
    $("#btnYeniEkle").click(function () {
        formuTemizle();
        $("#ogrenciModalBaslik").text("Yeni Öğrenci");
        $("#ogrenciModal").modal("show");
    });

    // Kaydet butonu (ekleme + güncelleme)
    $("#btnKaydet").click(function () {
        kaydet();
    });

    // Tablodaki Düzenle butonuna tıklanınca (event delegation)
    $("#tblOgrenci").on("click", ".btn-duzenle", function () {
        var id = $(this).data("id");
        duzenle(id);
    });

    // Tablodaki Sil butonuna tıklanınca (event delegation)
    $("#tblOgrenci").on("click", ".btn-sil", function () {
        var id = $(this).data("id");
        sil(id);
    });
    $("#btnSearch").click(function() {
        var searchTerm = $("#txtSearch").val();
        listeYukle(searchTerm);
    });
});

// Listeyi sunucudan çekip tabloyu doldurur
function listeYukle(searchTerm = "") {
    $.ajax({
        url: "/Ogrenci/GetAll",
        type: "GET",
        data: { search: searchTerm },
        dataType: "json",
        success: function (data) {
            tabloyuDoldur(data);
        },
        error: function () {
            alert("Öğrenci listesi yüklenirken bir hata oluştu.");
        }
    });
}

// Gelen veriyi tabloya yazar
function tabloyuDoldur(data) {
    if ($.fn.DataTable.isDataTable('#tblOgrenci')) {
        $('#tblOgrenci').DataTable().destroy();
    }

    var tbody = $("#tblOgrenci tbody");
    tbody.empty();

    $.each(data, function (i, item) {
        var satir = "<tr>" +
            "<td>" + item.OgrenciId + "</td>" +
            "<td>" + item.AdSoyad + "</td>" +
            "<td>" + item.Email + "</td>" +
            "<td>" +
            "<button type='button' class='btn btn-warning btn-sm btn-duzenle' data-id='" + item.OgrenciId + "'>Düzenle</button> " +
            "<button type='button' class='btn btn-danger btn-sm btn-sil' data-id='" + item.OgrenciId + "'>Sil</button>" +
            "</td>" +
            "</tr>";
        tbody.append(satir);
    });

    $('#tblOgrenci').DataTable({
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

// Ekle / Güncelle ortak kaydet fonksiyonu
function kaydet() {
    var id = parseInt($("#txtOgrenciId").val());

    var model = {
        OgrenciId: id,
        AdSoyad: $("#txtAdSoyad").val(),
        Email: $("#txtEmail").val()
    };

    var url = id === 0 ? "/Ogrenci/Create" : "/Ogrenci/Update";

    $.ajax({
        url: url,
        type: "POST",
        data: model,
        success: function (response) {
            if (response.success) {
                $("#ogrenciModal").modal("hide");
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

// Düzenleme için tek kaydı getirip modalı doldurur
function duzenle(id) {
    $.ajax({
        url: "/Ogrenci/GetById",
        type: "GET",
        data: { id: id },
        dataType: "json",
        success: function (item) {
            $("#txtOgrenciId").val(item.OgrenciId);
            $("#txtAdSoyad").val(item.AdSoyad);
            $("#txtEmail").val(item.Email);

            $("#ogrenciModalBaslik").text("Öğrenci Düzenle");
            $("#ogrenciModal").modal("show");
        },
        error: function () {
            alert("Kayıt getirilirken bir hata oluştu.");
        }
    });
}

// Silme işlemi
function sil(id) {
    if (!confirm("Bu öğrenciyi silmek istediğinize emin misiniz?")) {
        return;
    }

    $.ajax({
        url: "/Ogrenci/Delete",
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

// Modal her açıldığında formu sıfırlar
function formuTemizle() {
    $("#txtOgrenciId").val(0);
    $("#txtAdSoyad").val("");
    $("#txtEmail").val("");
}
