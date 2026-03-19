import { BASE_API } from "@/boot/axios";
import type { PagedResult, ProductToIndex, Result } from "@/models";

export function ProductService() {

  const CONTROLLER = '/Product';

  return {
    index: ({nameFilter, pageNumber, pageSize, orderBy, orderAsc}
    : { nameFilter: string, pageNumber: number, pageSize: number, orderBy: string, orderAsc: boolean}
    )=> BASE_API.get<Result<PagedResult<ProductToIndex>>>(`${CONTROLLER}?${(nameFilter) ? 'nameFilter='+nameFilter+'&' : ''}pageNumber=${pageNumber}&pageSize=${pageSize}&orderBy=${orderBy}&orderAsc=${orderAsc}`)
  }
}
