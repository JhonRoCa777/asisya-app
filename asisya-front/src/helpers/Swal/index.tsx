import Swal from "sweetalert2";

export const SwalHelper = {
  success: (msg: string) => Swal.fire({
    allowOutsideClick: false,
    icon: "success",
    title: "¡Listo!",
    text: msg
  }),

  info: (msg: string) => Swal.fire({
    allowOutsideClick: false,
    icon: "info",
    text: msg
  }),

  warning: (msg: string) => Swal.fire({
    allowOutsideClick: false,
    icon: "warning",
    title: "Oops...",
    text: msg
  }),

  error: (msg: string) => Swal.fire({
    allowOutsideClick: false,
    icon: "error",
    title: "Oops...",
    text: msg
  }),

  confirm: (msg: string, type: 'delete' | 'accept') => Swal.fire({
    allowOutsideClick: false,
    title: "¿Estás seguro?",
    html: msg,
    icon: "question",
    showCancelButton: true,
    confirmButtonColor: (type === 'delete') ? "var(--danger-color)" : "var(--tertiary-color)",
    confirmButtonText: (type === 'delete') ? "Borrar" : "Aceptar",
    cancelButtonText: "Cancelar"
  }),

  loading: () => Swal.fire({
    allowOutsideClick: false,
    title: 'Cargando...',
    text: 'Por favor espera.',
    didOpen: () => {
      Swal.showLoading();
    }
  }),
}
