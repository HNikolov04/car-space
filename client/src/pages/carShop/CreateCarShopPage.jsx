import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import AboutHeader from "../../components/shared/AboutHeader";
import Footer from "../../components/shared/Footer";
import useCarShopCreate from "../../hooks/useCarShopCreate";

export default function CreateCarShopPage() {
  const { register, handleSubmit, onSubmit, brands } = useCarShopCreate();
  const navigate = useNavigate();

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <AboutHeader />

      <main className="flex-1 flex items-center justify-center px-4 py-10">
        <div className="bg-white border border-gray-200 shadow-xl rounded-2xl p-8 w-full max-w-2xl scale-[0.97]">
          <h2 className="text-2xl font-bold text-center mb-6 text-gray-900">
            Create Car Listing
          </h2>

          <form onSubmit={handleSubmit(onSubmit)} className="space-y-5 text-sm">
            <div className="flex flex-col md:flex-row gap-4">
              <div className="w-full">
                <label className="block italic mb-1 text-gray-700">Title</label>
                <input
                  {...register("title", { required: true })}
                  type="text"
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                />
              </div>
              <div className="w-full">
                <label className="block italic mb-1 text-gray-700">Price</label>
                <input
                  {...register("price", { required: true })}
                  type="number"
                  step="0.01"
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                />
              </div>
            </div>

            <div>
              <label className="block italic mb-1 text-gray-700">Description</label>
              <textarea
                {...register("description", { required: true })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg resize-none"
              />
            </div>

            <div>
              <label className="block italic mb-1 text-gray-700">Upload Image</label>
              <input
                {...register("imageFile", { required: true })}
                type="file"
                accept="image/*"
                className="w-full cursor-pointer px-4 py-2 border border-gray-300 rounded-lg"
              />
            </div>

            <div>
              <label className="block italic mb-1 text-gray-700">Brand</label>
              <select
                {...register("brandId", { required: true })}
                className="cursor-pointer w-full px-4 py-2 border border-gray-300 rounded-lg"
                defaultValue=""
              >
                <option value="">Select a brand</option>
                {brands.map((brand) => (
                  <option key={brand.id} value={brand.id}>
                    {brand.name}
                  </option>
                ))}
              </select>
            </div>

            {[
              { name: "model", label: "Model", type: "text" },
              { name: "year", label: "Year", type: "number" },
              { name: "mileage", label: "Mileage", type: "number" },
              { name: "horsepower", label: "Horsepower", type: "number" },
              { name: "transmission", label: "Transmission", type: "text" },
              { name: "fuelType", label: "Fuel Type", type: "text" },
              { name: "color", label: "Color", type: "text" },
              { name: "euroStandard", label: "Euro Standard", type: "text" },
              { name: "doors", label: "Doors", type: "number" },
              { name: "city", label: "City", type: "text" },
              { name: "address", label: "Address", type: "text" },
            ].map(({ name, label, type }) => (
              <div key={name}>
                <label className="block italic mb-1 text-gray-700">{label}</label>
                <input
                  {...register(name, { required: true })}
                  type={type}
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                />
              </div>
            ))}

            <div className="flex justify-between pt-4">
              <button
                type="button"
                onClick={() => navigate(-1)}
                className="px-4 py-2 cursor-pointer bg-red-500 hover:bg-red-600 text-white rounded-md shadow"
              >
                Cancel
              </button>
              <button
                type="submit"
                className="px-4 py-2 cursor-pointer bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
              >
                Submit
              </button>
            </div>
          </form>
        </div>
      </main>

      <Footer />
    </div>
  );
}
