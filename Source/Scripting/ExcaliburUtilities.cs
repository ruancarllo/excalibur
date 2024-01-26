namespace ExcaliburUtilities {
  public class GeneralTools {
    public static System.Boolean ParseNumericText(System.String text, out System.Double result) {
      System.Boolean wasParsingSuccessfully = System.Double.TryParse(
        text.Replace(',', '.'),
        System.Globalization.NumberStyles.Any,
        System.Globalization.CultureInfo.InvariantCulture,
        out System.Double parsingResult
      );

      result = parsingResult;
      return wasParsingSuccessfully;
    }
  }
}