import { httpClient } from "./httpClient";

export async function getCarServiceCategories() {
  return await httpClient("api/CarServiceCategory");
}

export async function getCarServiceCategoryById(id) {
  return await httpClient(`api/CarServiceCategory/${id}`);
}

export async function createCarServiceCategory(data) {
  return await httpClient("api/CarServiceCategory/create", {
    method: "POST",
    data,
  });
}

export async function updateCarServiceCategory(data) {
  return await httpClient("api/CarServiceCategory", {
    method: "PUT",
    data,
    isForm: false,
  });
}

export async function deleteCarServiceCategory(id) {
  return await httpClient(`api/CarServiceCategory/${id}`, {
    method: "DELETE",
  });
}
