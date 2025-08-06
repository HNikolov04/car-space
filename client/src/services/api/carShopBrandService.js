import { httpClient } from "./httpClient";

const BASE = "api/CarShopBrand";

export async function getCarShopBrands() {
  return await httpClient(`${BASE}`);
}

export async function getCarShopBrandById(id) {
  return await httpClient(`${BASE}/${id}`);
}

export async function createCarShopBrand(data) {
  return await httpClient(`${BASE}`, {
    method: "POST",
    data,
  });
}

export async function updateCarShopBrand(id, data) {
  return await httpClient(`${BASE}/${id}`, {
    method: "PUT",
    data,
  });
}

export async function deleteCarShopBrand(id) {
  return await httpClient(`${BASE}/${id}`, {
    method: "DELETE",
  });
}
