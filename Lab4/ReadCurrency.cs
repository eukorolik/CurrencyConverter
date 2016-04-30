using System;
using System.Xml;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace Lab4
{
	public class ReadCurrency
	{
		readonly string Url;

		public ReadCurrency (string url)
		{
			Url = url;
		}

		public CurrencyList GetCurrencyList ()
		{
			var currencyXML = GetXMLFromSite ();

			double ruble2euro = 0, ruble2dollar = 0;

			foreach (XElement valute in currencyXML.Root.Elements ())
			{
				if (valute.Element ("CharCode").Value == "USD")
					ruble2dollar = Double.Parse (valute.Element ("Value").Value);

				if (valute.Element ("CharCode").Value == "EUR")
					ruble2euro = Double.Parse (valute.Element ("Value").Value);
			}

			return new CurrencyList (ruble2euro, ruble2dollar);
		}

		public XDocument GetXMLFromSite ()
		{
			XDocument result;

			try
			{
				var request = WebRequest.Create (Url);

				var stream = request.GetResponse().GetResponseStream();

				result = XDocument.Load (new StreamReader(stream, Encoding.GetEncoding (1251)));

				stream.Close ();
				request.Abort ();

				result.Save("currencyhash.xml");
			}
			catch (WebException)
			{
				result = XDocument.Parse (File.ReadAllText ("currencyhash.xml", Encoding.GetEncoding (1251)));
			}

			return result;
		}
	}

	public struct CurrencyList
	{
		public readonly double RubleToEuro;
		public readonly double RubleToDollar;
		public readonly double DollarToEuro;

		public CurrencyList (double ruble2euro, double ruble2dollar)
		{
			RubleToEuro = ruble2euro;
			RubleToDollar = ruble2dollar;
			DollarToEuro = RubleToEuro / RubleToDollar;
		}
	}
}