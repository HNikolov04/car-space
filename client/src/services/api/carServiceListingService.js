import { httpClient } from "./httpClient";

const BASE = "api/CarServiceListing";

export async function getCarServiceListings(params = {}) {
  const cleanedParams = Object.fromEntries(
    Object.entries(params).filter(
      ([_, value]) => value !== null && value !== undefined
    )
  );
  const query = new URLSearchParams(cleanedParams).toString();
  return await httpClient(`${BASE}?${query}`);
}

export async function getCarServiceListingById(id) {
  return await httpClient(`${BASE}/${id}`);
}

export async function createCarServiceListing(data) {
  const formData = new FormData();
  formData.append("Title", data.title);
  formData.append("Description", data.description);
  formData.append("CategoryId", data.categoryId);
  formData.append("PhoneNumber", data.phoneNumber);
  formData.append("City", data.city);
  formData.append("Address", data.address);
  formData.append("Price", data.price);
  if (data.imageFile) {
    formData.append("ImageFile", data.imageFile);
  }

  return await httpClient(BASE, {
    method: "POST",
    data: formData,
    isForm: true,
  });
}

export async function updateCarServiceListing(data) {
  const formData = new FormData();
  formData.append("Id", data.id);
  formData.append("Title", data.title);
  formData.append("Description", data.description);
  formData.append("CategoryId", data.categoryId);
  formData.append("PhoneNumber", data.phoneNumber);
  formData.append("City", data.city);
  formData.append("Address", data.address);
  formData.append("Price", data.price);
  if (data.imageFile) {
    formData.append("ImageFile", data.imageFile);
  }

  return await httpClient(`${BASE}/${data.id}`, {
    method: "PUT",
    data: formData,
    isForm: true,
  });
}

export async function deleteCarServiceListing(id) {
  return await httpClient(`${BASE}/${id}`, {
    method: "DELETE",
  });
}

export async function saveCarServiceListing(id) {
  return await httpClient(`${BASE}/${id}/save`, {
    method: "POST",
  });
}

export async function unsaveCarServiceListing(id) {
  return await httpClient(`${BASE}/${id}/unsave`, {
    method: "DELETE",
  });
}
