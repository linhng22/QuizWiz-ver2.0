namespace ProjectStudyTool;

public class VisionService {
    private readonly string vision_endpoint;
    private readonly string vision_key;

    public VisionService() {
        vision_endpoint = System.Configuration.ConfigurationManager.AppSettings["vision-endpoint"]!;
        vision_key = System.Configuration.ConfigurationManager.AppSettings["vision-key"]!;

        if (string.IsNullOrEmpty(vision_endpoint) || string.IsNullOrEmpty(vision_key)) {
            throw new ApplicationException("Vision configurations are incomplete.");
        }
    }

    public async Task<IReadOnlyList<string>> UseVisionService(IFormFile image) {
        ImageAnalysisClient client = new ImageAnalysisClient(
        new Uri(vision_endpoint),
        new AzureKeyCredential(vision_key));

        // Convert the image into a Stream
        using var stream = new MemoryStream();
        await image.CopyToAsync(stream);
        stream.Position = 0;

        // Convert the Stream to BinaryData in order to analyze the image
        BinaryData imageData = BinaryData.FromStream(stream);

        ImageAnalysisResult result = client.Analyze(
            imageData,
            VisualFeatures.Read,
            new ImageAnalysisOptions { GenderNeutralCaption = true });           
        
        List<string> analysisResults = new List<string>();
        StringBuilder sb = new StringBuilder();
        
        Console.WriteLine("Image analysis results:");
        Console.WriteLine(" Read:");
        if (result.Read.Blocks.Any())
        {
            foreach (DetectedTextBlock block in result.Read.Blocks)
                foreach (DetectedTextLine line in block.Lines)
                {
                    // Console.WriteLine($"   Line: '{line.Text}']");
                    sb.AppendLine(line.Text);
                }
            analysisResults.Add(sb.ToString());
        }
        else
        {
            analysisResults.Add("No text found in image.");
        }

        return analysisResults.AsReadOnly();
    }
}