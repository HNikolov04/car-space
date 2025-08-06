import { httpClient } from "./httpClient";

const BASE = "api/CarShopListing";

export async function getCarShopListings(params = {}) {
  const cleanedParams = Object.fromEntries(
    Object.entries(params).filter(
      ([_, value]) =>
        value !== null &&
        value !== undefined &&
        value !== "" &&
        !(typeof value === "number" && value === 0)
    )
  );

  const query = new URLSearchParams(cleanedParams).toString();
  return await httpClient(`${BASE}?${query}`);
}

export async function getCarShopListingById(id) {
  return await httpClient(`${BASE}/${id}`);
}

export async function createCarShopListing(data) {
  const formData = new FormData();
  formData.append("Title", data.title);
  formData.append("Description", data.description);
  formData.append("BrandId", data.brandId);
  formData.append("Model", data.model);
  formData.append("Year", data.year);
  formData.append("Mileage", data.mileage);
  formData.append("Horsepower", data.horsepower);
  formData.append("Transmission", data.transmission);
  formData.append("FuelType", data.fuelType);
  formData.append("Color", data.color);
  formData.append("EuroStandard", data.euroStandard);
  formData.append("Doors", data.doors);
  formData.append("Price", data.price);
  formData.append("City", data.city);
  formData.append("Address", data.address);
  if (data.imageFile) {
    formData.append("ImageFile", data.imageFile);
  }

  return await httpClient(BASE, {
    method: "POST",
    data: formData,
    isForm: true,
  });
}

export async function updateCarShopListing(data) {
  const formData = new FormData();
  formData.append("Id", data.id);
  formData.append("Title", data.title);
  formData.append("Description", data.description);
  formData.append("BrandId", data.brandId);
  formData.append("Model", data.model);
  formData.append("Year", data.year);
  formData.append("Mileage", data.mileage);
  formData.append("Horsepower", data.horsepower);
  formData.append("Transmission", data.transmission);
  formData.append("FuelType", data.fuelType);
  formData.append("Color", data.color);
  formData.append("EuroStandard", data.euroStandard);
  formData.append("Doors", data.doors);
  formData.append("Price", data.price);
  formData.append("City", data.city);
  formData.append("Address", data.address);
  if (data.imageFile) {
    formData.append("ImageFile", data.imageFile);
  }

  return await httpClient(`${BASE}/${data.id}`, {
    method: "PUT",
    data: formData,
    isForm: true,
  });
}

export async function deleteCarShopListing(id) {
  return await httpClient(`${BASE}/${id}`, {
    method: "DELETE",
  });
}

export async function saveCarShopListing(id) {
  return await httpClient(`${BASE}/${id}/save`, {
    method: "POST",
  });
}

export async function unsaveCarShopListing(id) {
  return await httpClient(`${BASE}/${id}/unsave`, {
    method: "DELETE",
  });
}
