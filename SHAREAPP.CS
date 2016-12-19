    var tapShare = new TapGestureRecognizer();
                        tapShare.Tapped += (s, e) =>
                        {
                            CrossShare.Current.Share(new ShareMessage
                            {
                                Text = data.WebNewsUrl,
                                Title = "Share"
                            },
                       new ShareOptions
                       {
                           ChooserTitle = "Chooser Title",
                           ExcludedUIActivityTypes = new[] { ShareUIActivityType.PostToFacebook }
                       });                            
                        };
                        imageShares.GestureRecognizers.Add(tapShare);
