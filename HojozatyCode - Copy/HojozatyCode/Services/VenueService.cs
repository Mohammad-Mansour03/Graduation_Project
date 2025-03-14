using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HojozatyCode.Models;
using Microsoft.Maui.Storage;
using Supabase;

namespace HojozatyCode.Services
{
    public static class VenueService
    {
        private const string BUCKET_NAME = "venue";
        private const string STORAGE_URL = "https://vkfjltultitueghevwhs.supabase.co/storage/v1/object/public/venue/";

        /// <summary>
        /// Creates a new venue record and uploads the provided images.
        /// </summary>
        /// <param name="venue">The venue model to save.</param>
        /// <param name="images">A list of images to upload.</param>
        /// <param name="categoryName">The name of the category to save.</param>
        /// <param name="categoryDescription">The description of the category to save.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        public static async Task<(bool Success, Guid? VenueId)> CreateVenueAsync(Venue venue, List<FileResult> images, string categoryName, string categoryDescription)
{
    if (venue == null)
        throw new ArgumentNullException(nameof(venue));

    try
    {
        // Upload all images and collect their public URLs
        List<string> imageUrls = await UploadImagesAsync(venue.VenueId, images);

        if (imageUrls.Count > 0)
        {
            // Combine URLs into a comma-separated string (or change this logic as needed)
            venue.ImageUrl = string.Join(",", imageUrls);
        }

        // Insert the venue record into the Supabase database
        var venueResponse = await SupabaseConfig.SupabaseClient
            .From<Venue>()
            .Insert(venue);

        if (venueResponse == null || venueResponse.Models.Count == 0)
        {
            Console.WriteLine("Failed to insert the venue record.");
                    return (false, null);
        }

        // Retrieve the inserted Venue to confirm the VenueId
        var insertedVenue = venueResponse.Models[0];

        // Save the category with the correct VenueId
        var category = new Category
        {
            CategoryId = Guid.NewGuid(),
                    VenueId = insertedVenue.VenueId,
            Name = categoryName,
            Description = categoryDescription
        };

        var categoryResponse = await SupabaseConfig.SupabaseClient
            .From<Category>()
            .Insert(category);

        if (categoryResponse == null || categoryResponse.Models.Count == 0)
        {
            Console.WriteLine("Failed to insert the category record.");
                    return (false, insertedVenue.VenueId);  // Return the venue ID even if category fails
        }

                return (true, insertedVenue.VenueId);  // Return both success status and venue ID
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error in CreateVenueAsync: {ex.Message}");
                return (false, null);
    }
}
        /// <summary>
        /// Uploads a list of images for the given venue.
        /// </summary>
        /// <param name="venueId">The ID of the venue (used for naming the files).</param>
        /// <param name="images">The list of images to upload.</param>
        /// <returns>A list of public URLs for the uploaded images.</returns>
        private static async Task<List<string>> UploadImagesAsync(Guid venueId, List<FileResult> images)
        {
            var urls = new List<string>();

            if (images == null || images.Count == 0)
                return urls;

            foreach (var image in images)
            {
                string url = await UploadImageAsync(image, venueId);
                if (!string.IsNullOrEmpty(url))
                {
                    urls.Add(url);
                }
                else
                {
                    Console.WriteLine($"Image upload failed for file: {image.FileName}");
                }
            }

            return urls;
        }

        /// <summary>
        /// Uploads a single image to Supabase Storage.
        /// </summary>
        /// <param name="image">The image file to upload.</param>
        /// <param name="venueId">The venue ID (used for file naming).</param>
        /// <returns>The public URL of the uploaded image, or null if the upload fails.</returns>
        private static async Task<string> UploadImageAsync(FileResult image, Guid venueId)
        {
            if (image == null)
            {
                Console.WriteLine("Image is null.");
                return null;
            }

            try
            {
                // Generate a unique filename using the venue ID and current timestamp
                string extension = Path.GetExtension(image.FileName)?.ToLower() ?? ".jpg";
                string fileName = $"{venueId}_{DateTime.UtcNow.Ticks}{extension}";

                // Read the file data into a byte array
                using var stream = await image.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;
                byte[] fileBytes = memoryStream.ToArray();

                Console.WriteLine($"Uploading image with filename: {fileName}");

                // Prepare upload options (including proper content type)
                var fileOptions = new Supabase.Storage.FileOptions
                {
                    ContentType = GetContentType(extension),
                    Upsert = true
                };

                // Upload the image to the specified bucket
                var uploadResult = await SupabaseConfig.SupabaseClient.Storage
                    .From(BUCKET_NAME)
                    .Upload(fileBytes, fileName, fileOptions);

                if (uploadResult != null)
                {
                    string publicUrl = $"{STORAGE_URL}{fileName}";
                    Console.WriteLine($"Successfully uploaded image: {publicUrl}");
                    return publicUrl;
                }
                else
                {
                    Console.WriteLine("Upload result was null.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Returns the MIME content type based on the file extension.
        /// </summary>
        private static string GetContentType(string extension)
        {
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                _ => "application/octet-stream",
            };
        }

        /// <summary>
        /// Fetches the list of venues with a pending status.
        /// </summary>
        /// <returns>A list of pending venues.</returns>
        public static async Task<List<Venue>> GetPendingVenuesAsync()
        {
            try
            {
                var response = await SupabaseConfig.SupabaseClient
                    .From<Venue>()
                    .Where(v => v.Status == "Pending")
                    .Get();
                Console.WriteLine($"Fetched {response.Models.Count} pending venues.");
                Console.WriteLine($"Venue details: {response.Models[0].VenueName}");
                return response.Models;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching pending venues: {ex.Message}");
                return new List<Venue>();
            }
        }

        /// <summary>
        /// Updates the status of a venue.
        /// </summary>
        /// <param name="venue">The venue to update.</param>
        /// <returns>True if the operation succeeds; otherwise, false.</returns>
        // public static async Task<bool> UpdateVenueStatusAsync(Venue venue)
        // {
        //     try
        //     {
        //         var response = await SupabaseConfig.SupabaseClient
        //             .From<Venue>()
        //             .Update(venue);

        //         return response.Models.Count > 0;
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error updating venue status: {ex.Message}");
        //         return false;
        //     }
        // }

        // /// <summary>
        // /// Deletes a venue.
        // /// </summary>
        // /// <param name="venue">The venue to delete.</param>
        // /// <returns>True if the operation succeeds; otherwise, false.</returns>
        // public static async Task<bool> DeleteVenueAsync(Venue venue)
        // {
        //     if (venue == null)
        //     {
        //         Console.WriteLine("Venue is null.");
        //         return false;
        //     }

        //     try
        //     {
        //         var response = await SupabaseConfig.SupabaseClient
        //             .From<Venue>()
        //             .Delete(venue);

        //         if (response == null)
        //         {
        //             Console.WriteLine("Response from Supabase is null.");
        //             return false;
        //         }

        //         if (response.Models == null)
        //         {
        //             Console.WriteLine("Response.Models is null.");
        //             return false;
        //         }

        //         return response.Models.Count > 0;
        //     }
        //     catch (Exception ex)
        //     {
        //         Console.WriteLine($"Error deleting venue: {ex.Message}");
        //         return false;
        //     }
        // }
    }
}
