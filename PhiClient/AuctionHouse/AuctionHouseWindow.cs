﻿using PhiClient.UI;
using PhiData.AuctionHouseSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace PhiClient
{
    class AuctionHouseWindow : Window
    {
        public int openedTab = 0;

        public override Vector2 InitialSize
        {
            get
            {
                return new Vector2(800, 1000);
            }
        }

        public override void PreOpen()
        {
            base.PreOpen();

            // We send a request to get the offer.
            // They won't be directly available, but they will at some time
            PhiClient.instance.SendPacket(new RequestOffersPacket());
        }

        public override void DoWindowContents(Rect inRect)
        {
            ListContainer cont = new ListContainer(ListFlow.COLUMN);
            cont.spaceBetween = ListContainer.SPACE;

            cont.Add(new TextWidget("Auction house", GameFont.Medium, TextAnchor.MiddleCenter));

            TabsContainer tabs = new TabsContainer(openedTab, (t) => openedTab = t);
            cont.Add(tabs);

            tabs.AddTab("Offers", DrawOffers());
            tabs.AddTab("Current offers", DrawOffers());
            tabs.AddTab("Make an offer", DrawOffers());

            cont.Draw(inRect);
        }

        public const float ROW_HEIGHT = 40f;

        public Displayable DrawOffers()
        {
            PhiClient phi = PhiClient.instance;

            ListContainer cont = new ListContainer(ListFlow.COLUMN);
            cont.drawAlternateBackground = true;

            Offer[] offers = phi.realmData.auctionHouse.offers.Where((o) => o.state == OfferState.OPEN).ToArray();
            foreach (Offer offer in offers)
            {
                Thing thing = phi.realmData.FromRealmThing(offer.realmThing);

                ListContainer row = new ListContainer(ListFlow.ROW);
                row.spaceBetween = ListContainer.SPACE;

                row.Add(new WidthContainer(new ThingIconWidget(thing), ROW_HEIGHT));
                row.Add(new WidthContainer(new TextWidget(offer.quantity.ToString()), 50f));
                row.Add(new TextWidget(thing.LabelCapNoCount));

                row.Add(new WidthContainer(new TextWidget(offer.price.ToString() + " silver"), 80f));

                cont.Add(row);
            }

            return cont;
        }

        public const float CREATE_OFFER_HEIGHT = 200f;

        public Displayable DrawCreateOffer()
        {
            ListContainer list = new ListContainer();
            list.spaceBetween = ListContainer.SPACE;

            return list;
        }
    }
}