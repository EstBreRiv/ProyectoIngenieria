function Delete(id) {
    Swal.fire({
        title: "¿Está seguro?",
        text: "El vehículo será desactivado permanentemente.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Sí, desactivar"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "/Vehiculo/Delete/" + id,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        setTimeout(() => {
                            window.location.href = '/Vehiculo/Index';
                        }, 2000);
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error("Error al procesar la solicitud");
                }
            });
        }
    });
}
