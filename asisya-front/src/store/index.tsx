import { configureStore } from "@reduxjs/toolkit";
import { useSelector } from "react-redux";
import { AuthSlice } from "./Auth";

interface AppStore {
  AuthSlice: any,
}

export const Store = configureStore<AppStore>({
  reducer: {
    AuthSlice: AuthSlice.reducer,
  }
});

export const AuthStore = () => useSelector((store: AppStore) => store.AuthSlice);