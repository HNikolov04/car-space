import { useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import AboutHeader from "../../components/shared/AboutHeader";
import Footer from "../../components/shared/Footer";
import useCreateCarServiceForm from "../../hooks/useCreateCarServiceForm";

export default function CreateCarServicePage() {
  const { register, handleSubmit, navigateBack, onSubmit, categories } = useCreateCarServiceForm();

  return (
    <div className="min-h-screen flex flex-col bg-gray-50">
      <AboutHeader />

      <main className="flex-1 flex items-center justify-center px-4 py-10">
        <div className="bg-white border border-gray-200 shadow-xl rounded-2xl p-8 w-full max-w-2xl scale-[0.97]">
          <h2 className="text-2xl font-bold text-center mb-6 text-gray-900">
            Create Car Service
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
                {...register("imageFile")}
                type="file"
                accept="image/*"
                className="w-full cursor-pointer px-4 py-2 border border-gray-300 rounded-lg"
              />
            </div>

            <div>
              <label className="block italic mb-1 text-gray-700">Category</label>
              <select
                {...register("categoryId", { required: true })}
                className="cursor-pointer w-full px-4 py-2 border border-gray-300 rounded-lg"
              >
                <option value="">Select a category</option>
                {categories.map((cat) => (
                  <option key={cat.id} value={cat.id}>
                    {cat.name}
                  </option>
                ))}
              </select>
            </div>

            <div>
              <label className="block italic mb-1 text-gray-700">Phone Number</label>
              <input
                {...register("phoneNumber", { required: true })}
                type="tel"
                className="w-full px-4 py-2 border border-gray-300 rounded-lg"
              />
            </div>

            <div className="flex flex-col md:flex-row gap-4">
              <div className="w-full">
                <label className="block italic mb-1 text-gray-700">City</label>
                <input
                  {...register("city", { required: true })}
                  type="text"
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                />
              </div>
              <div className="w-full">
                <label className="block italic mb-1 text-gray-700">Address</label>
                <input
                  {...register("address", { required: true })}
                  type="text"
                  className="w-full px-4 py-2 border border-gray-300 rounded-lg"
                />
              </div>
            </div>

            <div className="flex justify-between pt-4">
              <button
                type="button"
                onClick={navigateBack}
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
