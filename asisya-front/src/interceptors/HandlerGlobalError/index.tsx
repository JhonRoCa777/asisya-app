import { BASE_API } from "@/boot/axios";
import { SwalHelper } from "@/helpers";
import { ROUTER } from "@/router";

BASE_API.interceptors.response.use(

  (response) => {

    const apiResponse = response.data;
    let message = ''
    let code = 0;

    if(apiResponse?.error) 
    {
        code = apiResponse.error.code;
        message = apiResponse.error.message;
    }

    if(apiResponse?.Error)
    {
        code = apiResponse.Error.Code;
        message = apiResponse.Error.Message;
    }

    if (code > 0) {

      SwalHelper.error(message)
      .then ((value)=>{
        if(value.isConfirmed)
        {
          switch (code) {
            case 401:
              window.location.href = ROUTER.PRIVATE.fullPath;
              break;

            case 403:
              window.location.href = ROUTER.PUBLIC.fullPath;
              break;
          }
        }
      });

      return Promise.reject(apiResponse);
    }

    return response;
  },

  async (error) => {

    const message =
      error?.response?.data?.error?.message ||
      error?.response?.data?.message ||
      "Error inesperado";

    await SwalHelper.error(message);
      return await Promise.reject(error);
  }
);