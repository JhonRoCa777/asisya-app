import { Navigate, Outlet } from "react-router-dom"
import { ROLE_ENUM } from "@/models"
import { AuthStore } from "../../store"
import { ROUTER } from "../../router"

export const AdminGuard = () => {
  return (AuthStore().role === ROLE_ENUM.ADMIN) ? <Outlet/> : <Navigate replace to={ROUTER.HOME.MAIN}/>
}
