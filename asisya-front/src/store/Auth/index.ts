import { createSlice, type PayloadAction } from "@reduxjs/toolkit";

export const AuthSlice = createSlice({
  name: 'AuthSlice',
  initialState: {},
  reducers: {
    setAuthStore: ({}, action: PayloadAction<any>) => action.payload,
    resetAuthStore: () => {}
  }
});

export const {
  setAuthStore,
  resetAuthStore
} = AuthSlice.actions;
