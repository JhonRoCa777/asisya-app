import type { CategoryToRef } from "../Category";
import type { SupplierToRef } from "../Supplier";

export interface ProductToIndex {
    productId: string,
    productName: string,
    unitPrice: number,
    unitsInStock: number,
    unitsOnOrder: number,
    supplier: SupplierToRef,
    category: CategoryToRef
}