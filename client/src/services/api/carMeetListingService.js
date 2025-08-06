import { httpClient } from "./httpClient";

const BASE = "api/CarMeetListing";

export async function getCarMeets(params = {}) {
  const cleanedParams = Object.fromEntries(
    Object.entries(params).filter(
      ([_, value]) => value !== null && value !== undefined
    )
  );
  const query = new URLSearchParams(cleanedParams).toString();
  return await httpClient(`${BASE}?${query}`);
}

export async function getCarMeetById(id) {
  return await httpClient(`${BASE}/${id}`);
}

export async function getCarMeetParticipants(id, page = 1, pageSize = 10) {
  return await httpClient(`${BASE}/${id}/participants?page=${page}&pageSize=${pageSize}`);
}

export async function createCarMeetListing(data) {
  const formData = new FormData();
  formData.append("Title", data.title);
  formData.append("Description", data.description);
  formData.append("MeetDate", data.meetDate);
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

export async function updateCarMeetListing(data) {
  const formData = new FormData();
  formData.append("Id", data.id);
  formData.append("Title", data.title);
  formData.append("Description", data.description);
  formData.append("MeetDate", data.meetDate);
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

export async function deleteCarMeetListing(id) {
  return await httpClient(`${BASE}/${id}`, {
    method: "DELETE",
  });
}

export async function saveCarMeetListing(id) {
  return await httpClient(`${BASE}/${id}/save`, {
    method: "POST",
  });
}

export async function unsaveCarMeetListing(id) {
  return await httpClient(`${BASE}/${id}/unsave`, {
    method: "DELETE",
  });
}

export async function joinCarMeetListing(id) {
  return await httpClient(`${BASE}/${id}/join`, {
    method: "POST",
  });
}

export async function leaveCarMeetListing(id) {
  return await httpClient(`${BASE}/${id}/leave`, {
    method: "DELETE",
  });
}
