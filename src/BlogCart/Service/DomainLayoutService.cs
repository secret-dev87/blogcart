﻿using BlogCart.Shared.MainLayouts;


namespace BlogCart.Service
{
    public class DomainLayoutService
    {
        public Type GetLayoutForDomain(string domain)
        {
            if (domain == "mystore.lightningbits.com")
            {
                return typeof(LightningBitsLayout);
            }
            if (domain == "lightningbits.com")
            {
                return typeof(LightningBitsLayout);
            }
            else if (domain == "bluelemonz.com")
            {
                return typeof(BlueLemonZLayout);
            }
            else if (domain == "stefmancia.com")
            {
                return typeof(StefManciaLayout);
            }
            else if (domain == "thehealerisyou.com")
            {
                return typeof(TheHealerIsYouLayout);
            }
            //development change return to set path
            else if (domain == "localhost:7099")
            {
                return typeof(LightningBitsLayout);
            }
            else
            {
                return typeof(MainLayout);
            }
        }

    }

}

