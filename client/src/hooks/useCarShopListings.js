import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import { getCarShopListings } from "../services/api/carShopListingService";
import { useAuth } from "../contexts/AuthContext";

export default function useCarShopListings() {
  const { isAuthenticated } = useAuth();
  const [searchParams, setSearchParams] = useSearchParams();

  const page = parseInt(searchParams.get("Page") || "1");
  const pageSize = parseInt(searchParams.get("PageSize") || "10");
  const searchTerm = searchParams.get("SearchTerm") || "";
  const savedOnly = searchParams.get("SavedOnly") === "true";
  const myListingsOnly = searchParams.get("MyListingsOnly") === "true";

  const brandId = searchParams.get("BrandId");
  const title = searchParams.get("Title") || "";
  const minYear = searchParams.get("MinYear");
  const maxYear = searchParams.get("MaxYear");
  const minMileage = searchParams.get("MinMileage");
  const maxMileage = searchParams.get("MaxMileage");
  const minHorsepower = searchParams.get("MinHorsepower");
  const maxHorsepower = searchParams.get("MaxHorsepower");
  const minPrice = searchParams.get("MinPrice");
  const maxPrice = searchParams.get("MaxPrice");
  const transmission = searchParams.get("Transmission");
  const fuelType = searchParams.get("FuelType");
  const color = searchParams.get("Color");
  const euroStandard = searchParams.get("EuroStandard");
  const doors = searchParams.get("Doors");

  const [cars, setCars] = useState([]);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const updateParams = (params) => {
    const newParams = new URLSearchParams(searchParams);
    Object.entries(params).forEach(([key, val]) => {
      if (!val && val !== 0) {
        newParams.delete(key);
      } else {
        newParams.set(key, val);
      }
    });
    setSearchParams(newParams);
  };

  const handleFilterModeChange = (mode) => {
    updateParams({
      SavedOnly: mode === "saved" ? "true" : null,
      MyListingsOnly: mode === "mine" ? "true" : null,
      Page: "1",
    });
  };

  const handlePageChange = (newPage) => {
    updateParams({ Page: newPage.toString() });
  };

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      setError("");

      try {
        const response = await getCarShopListings({
          Page: page,
          PageSize: pageSize,
          SearchTerm: searchTerm || null,
          BrandId: brandId || null,
          Title: title || null,
          MinYear: minYear || null,
          MaxYear: maxYear || null,
          MinMileage: minMileage || null,
          MaxMileage: maxMileage || null,
          MinHorsepower: minHorsepower || null,
          MaxHorsepower: maxHorsepower || null,
          MinPrice: minPrice || null,
          MaxPrice: maxPrice || null,
          Transmission: transmission || null,
          FuelType: fuelType || null,
          Color: color || null,
          EuroStandard: euroStandard || null,
          Doors: doors || null,
          SavedOnly: isAuthenticated && savedOnly,
          MyListingsOnly: isAuthenticated && myListingsOnly,
        });

        setCars(response.items || []);
        setTotalPages(response.totalPages || 1);
      } catch (err) {
        console.error(err);
        setError("Failed to load car listings.");
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [
    page, pageSize, searchTerm, brandId, title,
    minYear, maxYear, minMileage, maxMileage,
    minHorsepower, maxHorsepower, minPrice, maxPrice,
    transmission, fuelType, color, euroStandard, doors,
    savedOnly, myListingsOnly, isAuthenticated
  ]);

  return {
    cars,
    totalPages,
    loading,
    error,
    searchTerm,
    updateParams,
    handlePageChange,
    handleFilterModeChange,
    filterParams: {
      brandId, title, minYear, maxYear, minMileage, maxMileage,
      minHorsepower, maxHorsepower, minPrice, maxPrice,
      transmission, fuelType, color, euroStandard, doors,
      savedOnly, myListingsOnly
    }
  };
}
