import { BASE_API } from "@/boot/axios";
import type { EmployeeLoginDTO } from "@/models";

export function AuthService() {

  const CONTROLLER = '/Auth';

  return {
    login: (credentials: EmployeeLoginDTO) => BASE_API.post(`${CONTROLLER}/Login`, credentials),
    verify: () => BASE_API.get(`${CONTROLLER}/Verify`),
    logout: async () => BASE_API.get(`${CONTROLLER}/Logout`)
  }
}
