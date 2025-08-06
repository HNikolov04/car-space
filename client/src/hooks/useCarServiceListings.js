import { useState, useEffect } from "react";
import { getCarServiceListings } from "../services/api/carServiceListingService";

export default function useCarServiceListings(searchParams, isAuthenticated) {
  const page = parseInt(searchParams.get("Page") || "1");
  const pageSize = parseInt(searchParams.get("PageSize") || "8");
  const searchTerm = searchParams.get("SearchTerm") || "";
  const savedOnly = searchParams.get("SavedOnly") === "true";
  const myServicesOnly = searchParams.get("MyServicesOnly") === "true";
  const minPrice = searchParams.get("MinPrice");
  const maxPrice = searchParams.get("MaxPrice");

  const [services, setServices] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError("");

      try {
        const data = await getCarServiceListings({
          Page: page,
          PageSize: pageSize,
          SearchTerm: searchTerm || null,
          MinPrice: minPrice || null,
          MaxPrice: maxPrice || null,
          SavedOnly: isAuthenticated && savedOnly,
          MyServicesOnly: isAuthenticated && myServicesOnly,
        });

        setServices(data.items || []);
        setTotalPages(data.totalPages || 1);
      } catch (err) {
        console.error(err);
        setError("Failed to fetch car services.");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [page, pageSize, searchTerm, savedOnly, myServicesOnly, minPrice, maxPrice, isAuthenticated]);

  return { services, totalPages, loading, error, page };
}
