# Tenancy Details

## Get tenancy agreement info for a tenancy

```sql
SELECT tenagree.tenure as tenure_type_code,
  tenure_lookup.ten_desc as tenure_type_display_name,
  tenagree.rent as current_rent,
  tenagree.service as current_service_charge,
  tenagree.other_charge as current_other_charge,
  tenagree.cot as tenancy_start_date,
  tenagree.eot as tenancy_end_date
FROM [dbo].[tenagree] tenagree
LEFT JOIN [dbo].[tenure] tenure_lookup
ON tenagree.tenure = tenure_lookup.ten_type
WHERE tenagree.tag_ref = '000075/01';
```

## Get arrears action diary events for a tenancy

```sql
SELECT araction.action_date as created_date,
  araction.action_code as type_code,
  raaction.act_name as type_display_name,
  araction.action_comment as description,
  araction.action_balance as balance,
  araction.username as user_name
FROM [dbo].[araction] araction
LEFT JOIN [dbo].[raaction] raaction
ON araction.action_code = raaction.act_code
WHERE tag_ref = '000075/01';
```

## Get agreements for a tenancy

```sql
SELECT arag.arag_status as status_code,
  status_lookup.lu_desc as status_display_name,
  arag.arag_startdate as start_date,
  aragdet.aragdet_frequency as frequency_code,
  frequency_lookup.lu_desc as frequency_display_name,
  aragdet.aragdet_amount as amount,
  arag.arag_breached as has_been_breached,
  arag.arag_comment as comment
FROM [dbo].[arag] arag
INNER JOIN [dbo].[aragdet] aragdet
ON arag.arag_sid = aragdet.arag_sid
LEFT JOIN [dbo].[lookup] frequency_lookup
ON aragdet.aragdet_frequency = frequency_lookup.lu_ref
LEFT JOIN [dbo].[lookup] status_lookup
ON arag.arag_status = status_lookup.lu_ref
WHERE arag.tag_ref = '000075/01'
AND frequency_lookup.lu_type = 'ZPS'
AND status_lookup.lu_type = 'AAS';
```

```json
{
  "ref": "✅",
  "current_balance": "🚫",
  "current_rent": "✅",
  "current_service_charge": "✅",
  "tenure_type": {
    "code": "✅",
    "display_name": "✅"
  },
  "start_date": "✅",
  "contacts": [{
    "full_name": "🚫",
    "mobile_phone_number": "🚫",
    "home_phone_number": "🚫",
    "email_address": "🚫"
  }],
  "property_address": {
    "address_1": "🚫",
    "address_2": "🚫",
    "address_3": "🚫",
    "address_4": "🚫",
    "address_5": "🚫",
    "post_code": "🚫",
  },
  "current_agreement": {
    "status": "✅",
    "date_issued": "✅",
    "amount": "✅",
    "frequency": "✅",
    "breached": "✅",
    "comment": "✅"
  },
  "number_of_agreements": "🚫",
  "recent_arrears_actions": [{
    "created_date": "✅",
    "type": {
      "code": "✅",
      "display_name": "✅"
    },
    "description": "✅",
    "user_name": "✅",
    "documents": ["???"]
  }],
  "number_of_arrears_actions": "🚫",
  "recent_transactions": [{
    "type": {
      "code": "🚫",
      "display_name": "🚫"
    },
    "date": "🚫",
    "payment_method": {
      "code": "🚫",
      "display_name": "🚫"
    },
    "delta": "🚫",
    "final_balance": "🚫"
  }],
  "number_of_transactions": "🚫",
}
```
