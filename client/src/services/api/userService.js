import { httpClient } from "./httpClient";

const BASE = "api/ApplicationUser";

export async function getCurrentUser() {
  return await httpClient(`${BASE}/me`);
}

export async function updateUserProfile({ newUsername, newPassword, confirmPassword, profileImage }) {
  const formData = new FormData();

  if (newUsername) formData.append("NewUsername", newUsername);
  if (newPassword) formData.append("NewPassword", newPassword);
  if (confirmPassword) formData.append("ConfirmPassword", confirmPassword);
  if (profileImage) formData.append("ProfileImage", profileImage);

  return await httpClient(`${BASE}/update`, {
    method: "PUT",
    data: formData,
    isForm: true,
  });
}

export async function deleteUser() {
  return await httpClient(`${BASE}/delete`, {
    method: "DELETE",
  });
}
