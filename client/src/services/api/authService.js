import { httpClient } from "./httpClient";

export async function register(data) {
  await httpClient("api/ApplicationUser/register", {
    method: "POST",
    data,
  });
}

export async function login(data) {
  const result = await httpClient("api/ApplicationUser/login", {
    method: "POST",
    data,
  });

  localStorage.setItem("token", result.jwtToken);
  return result;
}

export async function updateProfile(formData) {
  return await httpClient("api/ApplicationUser/update", {
    method: "PUT",
    data: formData,
    isForm: true,
  });
}

export async function deleteUser() {
  return await httpClient("api/ApplicationUser/delete", {
    method: "DELETE",
  });
}

export function logout() {
  localStorage.removeItem("token");
}
