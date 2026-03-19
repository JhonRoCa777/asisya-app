import type { PropsWithChildren } from "react";
import { Navigate, Route, Routes } from "react-router-dom";

interface IRoutesWithNotFound {
  navigateTo: string
}

export function RoutesWithNotFound({
  children, navigateTo
}: PropsWithChildren<IRoutesWithNotFound>) {
  return (
    <Routes>
      {children}
      <Route path='*' element={<Navigate to={navigateTo}/>}/>
    </Routes>
  )
}
