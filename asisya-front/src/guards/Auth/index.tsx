import { useEffect, useState } from "react";
import { Outlet } from "react-router-dom";
import { AuthService } from "@/services";
import { SpinnerFullScreen } from "@/components";

export const AuthGuard = () => {

  const [state, setState] = useState(false);

  const { verify } = AuthService();
  const getAuth = async () => {
    const Resp = await verify();
    setState(Resp.data.isSuccess);
  }

  useEffect(() => {
    getAuth();
  }, []);

  return (state) ? <Outlet/> : <SpinnerFullScreen/>
}
