import { httpClient } from "./httpClient";

export async function getCarForumBrands() {
  return await httpClient("api/CarForumBrand");
}

export async function getCarForumBrandById(id) {
  return await httpClient(`api/CarForumBrand/${id}`);
}

export async function createCarForumBrand(data) {
  return await httpClient("api/CarForumBrand", {
    method: "POST",
    data,
  });
}

export async function updateCarForumBrand(data) {
  return await httpClient("api/CarForumBrand", {
    method: "PUT",
    data,
  });
}

export async function deleteCarForumBrand(id) {
  return await httpClient(`api/CarForumBrand/${id}`, {
    method: "DELETE",
  });
}
