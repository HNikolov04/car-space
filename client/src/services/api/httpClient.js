const BASE_URL = import.meta.env.VITE_API_BASE_URL;

export function getToken() {
  return localStorage.getItem("token");
}

export async function httpClient(endpoint, { method = "GET", data, isForm = false } = {}) {
  const headers = {};

  if (!isForm) {
    headers["Content-Type"] = "application/json";
  }

  const token = getToken();
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }

  const options = {
    method,
    headers,
  };

  if (data) {
    options.body = isForm ? data : JSON.stringify(data);
  }

  const response = await fetch(`${BASE_URL}/${endpoint}`, options);

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(errorText || "Request failed");
  }

  if (response.status === 204) return null;

  const contentType = response.headers.get("content-type");
  if (contentType && contentType.includes("application/json")) {
    return await response.json();
  } else {
    return await response.text();
  }
}

export const get = (endpoint) => httpClient(endpoint, { method: "GET" });
export const post = (endpoint, data) => httpClient(endpoint, { method: "POST", data });
export const put = (endpoint, data) => httpClient(endpoint, { method: "PUT", data });
export const del = (endpoint) => httpClient(endpoint, { method: "DELETE" });
