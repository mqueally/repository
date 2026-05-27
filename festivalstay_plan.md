# FestivalStay.ie — Product Blueprint

## 1) Product architecture

### Product modules
- **Identity & Access**: signup/login, MFA optional, roles (Guest, Host, Organiser, Admin).
- **Festival CMS**: organiser-managed festival pages, dates, venue geolocation, branding assets, policy settings.
- **Listings & Availability**: host listings, amenities, nightly rates, festival-linked inventory, calendars.
- **Trust & Verification**: KYC/ID checks, optional Garda vetting workflow integration, host standards checklist, review moderation.
- **Search & Discovery**: multi-filter search, map-first browsing, distance/time-to-venue, saved favourites.
- **Booking & Payments**: booking lifecycle, Stripe payment intents, split payouts, refunds, disputes.
- **Messaging & Notifications**: in-app chat, email/SMS/push notifications, organiser broadcasts.
- **Organiser Console**: host approval, subsidy programs, analytics dashboards, capacity tracking.
- **Admin Console**: compliance tooling, fraud detection, case management, reporting.
- **Analytics & Insights**: occupancy, booking velocity, demand forecasting, economic impact estimations.

### Suggested technical architecture
- **Frontend web**: Next.js (App Router), Tailwind, component library (shadcn/ui).
- **Mobile app**: React Native (Expo) reusing design tokens and API contracts.
- **Backend**: Node.js (NestJS) or FastAPI; REST + selective GraphQL for dashboards.
- **Database**: PostgreSQL + PostGIS for geospatial queries.
- **Cache/queues**: Redis (sessions, rate limits, caching), SQS/RabbitMQ for async jobs.
- **Search**: PostgreSQL full-text initially; migrate to Meilisearch/Elastic for scale.
- **Storage**: S3-compatible object storage for photos/documents.
- **Payments**: Stripe Connect (marketplace model).
- **Auth**: Auth0/Clerk/Supabase Auth + custom RBAC.
- **Maps**: OpenStreetMap + Mapbox/Leaflet (cost-effective), Google Maps optional premium layer.
- **Observability**: OpenTelemetry + Datadog/Sentry.

### Bounded contexts
1. Accounts & Trust
2. Festivals
3. Listings
4. Bookings
5. Payments
6. Messaging
7. Reporting

---

## 2) Database schema (high-level)

### Core tables
- `users` (id, role, name, email, phone, locale, created_at)
- `profiles_host` (user_id, bio, verification_status, payout_account_id)
- `profiles_guest` (user_id, preferences_json)
- `organisations` (id, type: festival_org/council/tourism_board)
- `festivals` (id, org_id, name, slug, start_date, end_date, venue_geom, branding_json)
- `festival_partnership_settings` (festival_id, recommended_rate_min/max, commission_pct, subsidy_rules_json, host_requirements_json)
- `listings` (id, host_user_id, title, description, type, address, geom, max_guests, bedrooms, bathrooms, amenities_json, house_rules, status)
- `listing_festival_links` (listing_id, festival_id, approved_by_organiser, badge_level)
- `listing_photos` (id, listing_id, url, sort_order)
- `availability` (id, listing_id, date, is_available, nightly_rate, min_stay)
- `bookings` (id, listing_id, festival_id, guest_user_id, check_in, check_out, guests_count, status, total_amount)
- `booking_guests` (booking_id, guest_name, emergency_contact)
- `payments` (id, booking_id, stripe_payment_intent_id, gross, platform_fee, organiser_fee, host_payout, currency, status)
- `payouts` (id, host_user_id, stripe_transfer_id, amount, scheduled_at, paid_at)
- `subsidies` (id, festival_id, booking_id, subsidy_amount, payer_org_id, status)
- `messages_threads` / `messages`
- `reviews` (id, booking_id, reviewer_id, reviewee_id, rating, text)
- `verification_checks` (id, user_id, type, provider, status, evidence_url)
- `disputes` (id, booking_id, opened_by, category, status, resolution)
- `notifications` (id, user_id, channel, template_key, payload_json, sent_at)
- `audit_logs` (actor_id, action, entity, entity_id, metadata_json, created_at)

### Important indexes
- PostGIS index on `listings.geom`, `festivals.venue_geom`
- Composite indexes on `availability(listing_id, date)`
- Search indexes for `festivals(name)`, `listings(title, type)`

---

## 3) User journeys

### A) Host onboarding
1. Create account → verify email/phone.
2. Complete profile + ID verification.
3. Add listing details/photos and festival availability.
4. Accept host standards checklist.
5. Link Stripe payout account.
6. Submit for organiser verification (if applicable).
7. Go live and receive bookings.

### B) Guest booking
1. Select festival + dates.
2. Browse map/list + filters.
3. View listing details, reviews, distance, transport hints.
4. Message host if needed.
5. Book + pay securely.
6. Receive itinerary, contact details, and check-in instructions.
7. Post-stay review.

### C) Organiser workflow
1. Create official festival page.
2. Set pricing recommendations and host quality standards.
3. Invite/approve local hosts.
4. Monitor occupancy and demand shortfall.
5. Trigger subsidy/guarantee campaigns.
6. Send broadcast updates to hosts/guests.

### D) Admin operations
- Review flagged accounts/bookings.
- Handle disputes and refunds.
- Monitor fraud scores and unusual payout patterns.
- Manage compliance and data requests.

---

## 4) MVP roadmap (6 months)

### Phase 1 (Weeks 1–6): Foundations
- Auth + RBAC
- Festival pages
- Host listing creation
- Search + map (basic)

### Phase 2 (Weeks 7–12): Marketplace core
- Availability calendar
- Booking flow
- Stripe checkout + split payments
- Messaging MVP

### Phase 3 (Weeks 13–18): Trust and organiser tools
- ID verification integration
- Organiser approvals and host badges
- Reviews
- Basic analytics dashboard

### Phase 4 (Weeks 19–24): Pilot readiness
- Subsidy workflows
- Notification engine
- GDPR exports/deletion
- Performance hardening + support runbooks

Pilot target: **Willie Clancy + Doolin Folk + Ennis Trad**.

---

## 5) UI wireframe ideas

1. **Home page**: festival search hero, “Stay with locals” trust messaging, featured festivals.
2. **Festival page**: banner, dates, map radius, available homes, organiser-verified badge.
3. **Search results**: split view (list + map), sticky mobile filters, distance chips.
4. **Listing page**: photo carousel, amenity grid, host profile, standards badge, instant book CTA.
5. **Booking checkout**: transparent fee breakdown (host / platform / festival).
6. **Host dashboard**: calendar heatmap, earnings cards, pending approvals.
7. **Organiser dashboard**: occupancy gauge, capacity gap alerts, subsidy campaign controls.
8. **Admin console**: fraud queue, dispute board, verification queue.

---

## 6) Branding concepts

### Brand directions
- **“FestivalStay.ie”**: clear and functional.
- **“FáilteStay”**: culturally warm; “fáilte” conveys welcome.
- **“Ceol & Stay”**: music-forward for trad audiences.

### Visual style
- Palette: Atlantic blue, warm cream, moss green.
- Typography: modern sans with soft rounded headings.
- Imagery: real local homes, community faces, festival scenes.
- Tone: welcoming, trustworthy, practical.

Tagline options:
- “Stay Local. Feel the Festival.”
- “Where Festivals Meet Local Welcome.”
- “More Beds, More Music, More Community.”

---

## 7) Revenue model

1. **Booking commission** (e.g., 8–12% blended take rate).
2. **Festival SaaS subscription** for organiser dashboard + analytics.
3. **Featured listings** for hosts during high-demand windows.
4. **Subsidy administration fee** for managed incentive programs.
5. **Insurance/add-on partnerships** (optional protection packages).
6. **Tourism sponsorship placements** on festival pages.

---

## 8) Irish legal/compliance considerations

- GDPR: lawful basis, consent management, data minimisation, DSR workflow, DPA with vendors.
- Consumer law: transparent fees, cancellation/refund terms, clear contract party definitions.
- Payment compliance: PSD2/SCA via Stripe; AML considerations for payouts.
- Tax: host tax guidance, possible DAC7 reporting obligations.
- Platform liability: clear terms, host responsibility clauses, safety policy.
- Equality/non-discrimination policy for hosts.
- Optional Garda vetting: use vetted partner flow where legally applicable.
- Cookie compliance and analytics consent.

(Validate all with Irish counsel before launch.)

---

## 9) Mobile app considerations

- Prioritise guest booking funnel and host calendar updates.
- Offline-tolerant message drafts/check-in notes.
- Push notifications for booking status and organiser alerts.
- One-thumb UX for filters/checkout.
- Camera-native upload for hosts.
- Biometric login for returning users.

---

## 10) Scalable SaaS architecture (Ireland → Europe)

### Multi-tenant model
- Tenant = organiser/festival network.
- Shared infrastructure with strict row-level tenancy isolation.
- Tenant-configurable branding, pricing rules, policy templates.

### Scale strategy
- Start single-region EU (Ireland) with CDN.
- Expand to multi-region read replicas.
- Event-driven analytics pipeline (Kafka/Kinesis later stage).
- Feature flags per tenant/festival.

### Internationalisation readiness
- Multi-currency support.
- VAT/tax rule abstraction layer.
- i18n content system.
- Local trust workflow adapters (country-specific verification).

---

## Domain name ideas
- FestivalStay.ie
- FailteStay.ie
- StayTheFestival.ie
- FestiBeds.ie
- TradStay.ie
- CeolStay.ie
- LocalFestivalBeds.ie

---

## Marketing strategy

1. **Festival-first B2B2C motion**: sign organisers first, then recruit hosts using organiser credibility.
2. **Local host drives**: “Open your spare room for festival week” campaigns via parish/community groups.
3. **Trust-led creative**: verified host badges, real stories, safety-first messaging.
4. **Content**: festival accommodation guides (SEO) by town/festival/date.
5. **Referral loops**: guest invites + host referral bonuses.
6. **PR**: rural economic impact narrative with county data.

---

## County Clare pilot launch plan

### Pre-launch (8–12 weeks)
- Secure 2–3 organiser MOUs.
- Recruit first 150 hosts across Ennis, Miltown Malbay, Doolin, Lisdoonvarna.
- Run host onboarding workshops with local community centres.

### Launch wave
- Festival-specific landing pages.
- Early-bird guarantee/subsidy program funded by organisers or tourism grants.
- On-call trust & safety support during festival weekends.

### Post-pilot KPIs
- Active listings
- Occupancy rate
- Conversion from search to booking
- Avg distance to venue
- Guest NPS / host NPS
- Estimated local spend uplift

---

## Partnership targets
- **Local councils**: Clare County Council, Galway City/County Council.
- **Tourism bodies**: Fáilte Ireland, local tourism networks, chambers of commerce.
- **Transport partners**: Bus Éireann/local shuttle operators.
- **Education/community**: ETBs/community centres for host training.
- **Insurance/legal partners**: short-stay cover and standardised host policies.

