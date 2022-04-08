// See https://aka.ms/new-console-template for more information
public class JWT
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int[] Ids { get; set; }
}