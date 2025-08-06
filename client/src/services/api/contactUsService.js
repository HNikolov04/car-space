import { httpClient } from "./httpClient";

export async function getContactUs() {
  return await httpClient("api/ContactUs");
}

export async function updateContactUs(data) {
  return await httpClient("api/ContactUs", {
    method: "PUT",
    data,
  });
}