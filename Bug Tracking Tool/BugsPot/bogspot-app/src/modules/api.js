export async function fetchApi({ path, method = "GET", body }) {
  const options =
    method.toUpperCase() === "GET"
      ? {}
      : {
          body: JSON.stringify(body),
        };

  const response = await fetch(`http://localhost:5000/${path}`, {
    method,
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    credentials: "include",
    ...options,
  });

  return await response.json();
}
