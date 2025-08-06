import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import AboutHeader from "../../components/shared/AboutHeader";
import Footer from "../../components/shared/Footer";
import { useCarForumCreate } from "../../hooks/useCarForumCreate";

export default function CreateCarForumPage() {
  const navigate = useNavigate();
  const { register, handleSubmit, reset } = useForm();
  const {
    brands,
    error,
    loading,
    handleCreate,
  } = useCarForumCreate(reset);

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <AboutHeader />

      <main className="flex-1 flex items-center justify-center px-4 py-10">
        <div className="bg-white border border-gray-200 shadow-xl rounded-2xl p-7 w-full max-w-lg scale-[0.95]">
          <h2 className="text-2xl font-bold text-center mb-6 text-gray-900">
            Create Forum Article
          </h2>

          <form onSubmit={handleSubmit(handleCreate)} className="space-y-4 text-sm">
            <div>
              <label className="block italic mb-1 text-gray-700">Title</label>
              <input
                {...register("title", { required: true })}
                type="text"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-300"
              />
            </div>

            <div>
              <label className="block italic mb-1 text-gray-700">Description</label>
              <textarea
                {...register("description", { required: true })}
                className="w-full px-4 py-2 border border-gray-300 rounded-lg resize-none focus:outline-none focus:ring-2 focus:ring-blue-300"
              />
            </div>

            <div>
              <label className="block italic mb-1 text-gray-700">Brand</label>
              <select
                {...register("brandId", { required: true })}
                className="w-full cursor-pointer px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-300"
              >
                <option value="">Select a brand</option>
                {brands.map((brand) => (
                  <option key={brand.id} value={brand.id}>
                    {brand.name}
                  </option>
                ))}
              </select>
            </div>

            {error && (
              <div className="text-red-600 text-sm text-center font-medium">{error}</div>
            )}

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
                disabled={loading}
                className="px-4 py-2 cursor-pointer bg-blue-600 hover:bg-blue-700 text-white rounded-md shadow"
              >
                {loading ? "Submitting..." : "Submit"}
              </button>
            </div>
          </form>
        </div>
      </main>

      <Footer />
    </div>
  );
}
