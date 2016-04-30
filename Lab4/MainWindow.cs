using System;
using Gtk;
using Lab4;

public partial class MainWindow: Gtk.Window
{
	CurrencyList Currencies;
	enum Valutes { Ruble, Euro, Dollar }

	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
		var currency = new ReadCurrency ("http://www.cbr.ru/scripts/XML_daily.asp");
		Currencies = currency.GetCurrencyList ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnConverButtonClicked (object sender, EventArgs e)
	{
		string inputStr = FromInputLabel.Text;

		if (inputStr.Length == 0)
			return;

		if (inputStr.Contains ("."))
			inputStr = inputStr.Replace (".", ",");

		double input = Double.Parse (inputStr);

		switch (FromCurrency.Active) {
		case (int)Valutes.Ruble:
			{
				switch (ToCurrency.Active) {
				case (int)Valutes.Euro:
					ToInputLabel.Text = (input * (1 / Currencies.RubleToEuro)).ToString ();
					break;

				case (int)Valutes.Dollar:
					ToInputLabel.Text = (input * (1 / Currencies.RubleToDollar)).ToString ();
					break;
				}
			}
			break;

		case (int)Valutes.Euro:
			{
				switch (ToCurrency.Active) {
				case (int)Valutes.Ruble:
					ToInputLabel.Text = (input * Currencies.RubleToEuro).ToString ();
					break;

				case (int)Valutes.Dollar:
					ToInputLabel.Text = (input * Currencies.DollarToEuro).ToString ();
					break;
				}
			}
			break;

		case (int)Valutes.Dollar:
			{
				switch (ToCurrency.Active) {
				case (int)Valutes.Ruble:
					ToInputLabel.Text = (input * Currencies.RubleToDollar).ToString ();
					break;

				case (int)Valutes.Euro:
					ToInputLabel.Text = (input * (1 / Currencies.DollarToEuro)).ToString ();
					break;
				}
			}
			break;
		}
	}

	protected void OnFromCurrencyChanged (object sender, EventArgs e)
	{
		if (FromCurrency.Active == ToCurrency.Active)
		{
			ToForbidden.Show ();
			ToArrow.Hide ();
			ConvertButtonHide ();
		}
		else if (ToForbidden.Visible)
		{
			ToForbidden.Hide ();
			ToArrow.Show ();
			ConvertButtonShow ();
		}
	}

	protected void OnToCurrencyChanged (object sender, EventArgs e)
	{
		if (FromCurrency.Active == ToCurrency.Active)
		{
			ToForbidden.Show ();
			ToArrow.Hide ();
			ConvertButtonHide ();
		}
		else if (ToForbidden.Visible)
		{
			ToForbidden.Hide ();
			ToArrow.Show ();
			ConvertButtonShow ();
		}
	}

	protected void OnFromInputLabelChanged (object sender, EventArgs e)
	{
		double result = 0;
		string input = FromInputLabel.Text;

		if (input.Contains ("."))
			input = input.Replace (".", ",");

		if (!Double.TryParse (input, out result))
		{
			ConvertButtonHide ();
			FromInputError.Show ();
		}
		else if (!ConvertButton.Visible)
		{
			ConvertButtonShow ();
			FromInputError.Hide ();
		}
	}

	protected void ConvertButtonHide ()
	{
		ConvertButton.Hide ();
		ConvertButtonIcon.Hide ();
	}

	protected void ConvertButtonShow ()
	{
		ConvertButton.Show ();
		ConvertButtonIcon.Show ();
	}

}