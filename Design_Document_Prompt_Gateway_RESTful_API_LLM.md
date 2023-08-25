# Design Document: Prompt Gateway for External LLM with RESTful API and Plugin Support
## *Design Document Sub-Title: &#xA;&quot;Designing a Prompt Gateway with RESTful API Integration, Plugin-based Prompt Transformations, and Admin Configuration for an External LLM&quot;*

## *Abstract*
*The design document outlines the creation of a Prompt Gateway for an external LLM. The Prompt Gateway will be accessible to clients through a specific RESTful API URL. Its main function is to reliably accept prompts, apply optional transformations, and send the transformed prompts to the LLM. To achieve this, the Prompt Gateway will utilize any number of external and/or internal Plugin modules for prompt transformations. The order of these transformations will be determined by an internal rule engine. An Admin will have the ability to configure the Prompt Gateway with any number of Plugins. These Prompt Transformation Plugins will be accessible to the Prompt Gateway only through their respective RESTful endpoint URLs.*&#xA;**&#xA;*In addition to the development of the Prompt Gateway, the design document also specifies the creation of a local startup shell script. This script will be used to initiate the Prompt Gateway on a local machine. Furthermore, deployment artifacts for Ansible and Docker will be created to facilitate the deployment of the Prompt Gateway in different environments. Finally, comprehensive documentation will be produced to cover various aspects such as usage instructions, plugin configuration guidelines, and troubleshooting tips. This documentation will serve as a valuable resource for users and administrators of the Prompt Gateway.*

## *Task*
```Task
Create a Prompt Gateway for an external LLM with these specifications:&#xA;- Prompt Gateway is accessible to clients via a specific RESTful API URL.&#xA;- Prompt gateway reliably accepts prompts, then applies optional transformations, and sends transformed prompts to the LLM.&#xA;- Prompt Gateway handles Prompt Transformations via any number of external and/or internal Plugin modules.&#xA;- Prompt Gateway uses an internal rule engine to determine the order of the Prompt Transformations by the Plugins.&#xA;- An Admin can configure the Prompt Gateway with any number of Plugins.&#xA;- Prompt Transformation Plugins are accessible to the Prompt Gateway only via their respective RESTful endpoint URLs.&#xA;&#xA;Provide a local startup shell script.&#xA;Create deployment artifacts for Ansible and Docker.&#xA;Produce comprehensive documentation: usage, plugin configuration, and troubleshooting.
```

## *Design*


### *Use Cases*

```plantuml
@startuml
left to right direction
actor "API Caller" as API
actor "Admin" as Admin
rectangle "Sending Prompts to the Gateway" as UC1 {
  API -- (Send Prompts)
}
rectangle "Handling Prompt Transformations" as UC2 {
  (Handle Transformations) .> (Send Prompts) : <<extends>>
}
rectangle "Configuring the Prompt Gateway" as UC3 {
  Admin -- (Configure Gateway)
}
rectangle "Accessing Transformation Plugins" as UC4 {
  (Access Plugins) .> (Configure Gateway) : <<uses>>
}
@enduml
```


![Use Cases](https://www.plantuml.com/plantuml/dsvg/P951JiCm44NtFiKegtPHf1PMg8fQPK7PHG8EC4ecgIN7HlP4W10dOy6Hk08Pkmaax6pFd___axy_lmw1qNFV6MMeOM07NhTdXbfxgbWxgx1Yvo4xblTGe37aCy00mtMQr9sswL5EIXRHjeOWUo1RQzj2wLttp47i-KnmXqml-1Ptdeejl2iGGzXiO2KhetwjFlwwdT3MvjSE7ZtQq3ZVeHGD5xjTj5j5CIqrQxZA5nbm2_izlVBm5l9ydbWus-Yszn8gnI_1y_xNgR-mHu9nQU9SO1ohYa8GotavA4tVwedZ9d4azJXB_VydH8W-K28u32p3b_u0003__mC0)

  



## Details

## Sending Prompts to the Gateway
Description:
*API Callers send prompts to the Prompt Gateway which are then transformed and sent to the LLM. *

### *Actors:*
- API Caller

### *Pre-conditions:*
- The API Caller is connected to the Prompt Gateway.

### *Post-conditions:*
- The transformed prompts are sent to the LLM.


### *Flow:*
- The API Caller sends a prompt to the Prompt Gateway.- The Prompt Gateway applies optional transformations to the prompt.- The transformed prompt is sent to the LLM.



### *Robustness*

```plantuml
@startuml
title Sending Prompts to the Gateway Robustness Diagram

actor "API Caller" as A <<actor>>
boundary "Prompt Gateway API" as B <<boundary>>
control "Prompt Gateway" as C <<control>>

A -> B : Sends a prompt
B -> C : Applies optional transformations to the prompt
C -> A : Sends the transformed prompt to the LLM
@enduml
```


![Robustness](https://www.plantuml.com/plantuml/dsvg/N911QiCm44NtEiNWVIvGWedYGYaa498JJ68r5SWQCJCcv6nTz4YvGiQguqMt_x_taURnyxiHHTPnI4wZfW2daFkOht1W6eeAA85-1_X03JUymv7EesWE8l0UySeuE8SN9OR67pwXmvG2Du027jRhshIjEzEOU-GxD7-povmv5TuQ_8AClr1MflGVhsXdwDmRwJoiMjFVwiC544fLt7RAEyjzAIa60IeQAMC2PSpoHJpW52m_dBrkylooDpMB4FgPUZcxtTvjZBFpFG400F__0m00)

  




### *Sequence*

```plantuml
@startuml
title Sending Prompts to the Gateway Sequence Diagram

actor "API Caller" as A
entity "Prompt Gateway API" as B
control "Prompt Gateway" as C
entity "LLM" as D

A -> B : Sends a prompt
activate B
B -> C : Applies optional transformations to the prompt
activate C
C -> D : Sends the transformed prompt
activate D
D --> C : Acknowledgement
deactivate D
C --> B : Acknowledgement
deactivate C
B --> A : Acknowledgement
deactivate B

@enduml
```


![Sequence](https://www.plantuml.com/plantuml/dsvg/V95DQWCn34RtEeMOVIxGHScC1qeXXK1F86RKgQd_OYiDELiMELAlKDcbGqaXM-bzpv_mr-MwBK9PZjw1M76Chnnc6nRSv-YJ59I8yixuHC8dEklwSEGmCOwMbamUW2Q96Rj-_uo6dEFS8HNiWOCApzZzY5Q1SWqOO8f1SdItHDkQDRxRlRJ92D3ZvX47V6WT2nAc5gmDxAU6rJbKn2ZIf-Gi5un9R0paK3A5yXQpfpfOxxfL631LCQwlL6ZDylmlCCA8czz7fuyGJuxdXRtsXvd_OAPXmttCr0iKw-zZ0y1Ms-clVGC00F__0m00)

  




### *Activity*

```plantuml
@startuml
title Sending Prompts to the Gateway Activity Diagram

start
:API Caller is connected to the Prompt Gateway;

if (API Caller sends a prompt to the Prompt Gateway) then (yes)
  :Prompt Gateway applies optional transformations to the prompt;
  :Transformed prompt is sent to the LLM;
else (no)
  :API Caller does not send a prompt;
endif

stop
@enduml
```


![Activity](https://www.plantuml.com/plantuml/dsvg/R911QiCm44NtEiLVEbU8DmaDb402WVO2WprE1R96QAODFbiMFLAlKDcEJGfTQlJ__3V_v_wzKwDidtrmvYqmtZYsFbvnoT8dKvZ0FXYlP7oZ0Vl6_9Ut0GTFrqozSvFVRVUN8rue1CxmYaPYvCQuNVppkcLDxPplK3rvjCGg26dM_UlQZBE8Qc3TE63xznUKKl2ia6HU8WLOfgYTv9x6mNhA756F6zuNHG5z91Vumh8Id4xdsd5GHXLbIds2RgN4HR69VeKl-b9XDpOZoUtAexJx2m00__y30000)

  




### *State*

```plantuml
@startuml
title Sending Prompts to the Gateway State Diagram

state "API Caller Connected" as S1
state "Prompt Sent to Gateway" as S2
state "Prompt Transformed" as S3
state "Transformed Prompt Sent to LLM" as S4

[*] --> S1
S1 --> S2 : API Caller sends a prompt
S2 --> S3 : Prompt Gateway applies transformations
S3 --> S4 : Transformed prompt is sent to LLM
S4 --> [*]

@enduml
```


![State](https://www.plantuml.com/plantuml/dsvg/N91DQWCn34RtEiL7be4iyhFgegIa40ef1AQxqeLetDJWiGTBeVHiNVH8lKBbSGn3TZ9wUlhalv_VhIXdlVI1r6jmw5my-NZ6CQT-K84cwAV3djLzyJSwjGAFdi-PUo8PdxE7unDs78BBsAKOtRkwqmmiw9ODgRwYr-Ay-Ygqke5UCaVvIBblbdK39XtS60-7vmflY5xdRrWixilwRbch5UumYIbsfe0nZ1AozaYjZRfwsyKy3C4x-uYscjMdA6JiEBAnaMciQeINigCb8yCARS68jhRRFlmV003__mC0)

  




### *Class*

```plantuml
@startuml
title Class Diagram for Sending Prompts to the Gateway

class APICaller {
  -connectionStatus: boolean
  +sendPrompt(prompt: Prompt): void
}

class PromptGateway {
  -prompt: Prompt
  -transformedPrompt: Prompt
  +receivePrompt(prompt: Prompt): void
  +applyTransformations(): void
  +sendTransformedPrompt(): void
}

class Prompt {
  -content: String
  -transformedContent: String
  +getContent(): String
  +getTransformedContent(): String
  +setContent(content: String): void
  +setTransformedContent(transformedContent: String): void
}

class LLM {
  +receivePrompt(prompt: Prompt): void
}

APICaller "1" -- "1" PromptGateway : Sends >
PromptGateway "1" -- "1" Prompt : Transforms >
PromptGateway "1" -- "1" LLM : Sends >
@enduml
```


![Class](https://www.plantuml.com/plantuml/dsvg/Z59BQiCm4Dth54DMBiQBRhqeAIwKGWe1-m9JP-eAPCb8auGGUh8kUgHUeV8dSMGteQk1D_C-CzBFr_VICLhUragmP4MGANGEdYHM5cjuDnPoqXkfArXRKpViW0tm1y4pCktn84JPCHxNBnagHHQE0Y0fZTPKiZGwP-IjI-7D64MeVJDsNh5NYvgkf8FuNGex8pVYywJQmuDLhnmoMeGjQkUJrZIeNZHZIoN97TsqysFODEfGd8Im3UwYYtuRkPZwH5Voda_0fBrPpjRVRv8qctNZYdX0M-40BsR4SCIDr8bjiCDVEjSppRPRhLwxrVvrLCyQ_yJYVW59qfNmITFkUpbu424-8_Z9S_YRust8KNNfg__Slm000F__0m00)

  

## Handling Prompt Transformations
Description:
*The Prompt Gateway handles prompt transformations via external and/or internal Plugin modules. *

### *Actors:*
- Prompt Gateway

### *Pre-conditions:*
- The Prompt Gateway has received a prompt.

### *Post-conditions:*
- The prompt has been transformed by the Plugin modules.


### *Flow:*
- The Prompt Gateway receives a prompt.- The Prompt Gateway uses the internal rule engine to determine the order of transformations.- The Prompt Gateway sends the prompt to the Plugin modules for transformation.



### *Robustness*

```plantuml
@startuml
title Handling Prompt Transformations Robustness Diagram

actor "Prompt Gateway" as A <<actor>>
boundary "Plugin RESTful endpoints" as B <<boundary>>
control "Internal Rule Engine" as C <<control>>

A -> C : Receives a prompt
C -> A : Determines the order of transformations
A -> B : Sends the prompt to the Plugin modules for transformation
@enduml
```


![Robustness](https://www.plantuml.com/plantuml/dsvg/N93DRSCm34RlceBmFGiKWH7d1srkXPC569js1KYa8L4DCbiF7QahbBNTGtkK-Bs7Z_pw-DnbnQGb1gTU0y4BSXyyZ_2Q94uAruIS1qaHrGjdQELMiZBb34UFOyBe77Og2QgLU4QbEpugm0mDRBVpjAxTJGhtc1uM36Nq3EtfSXrA0E9-4i-QPsHlo6_Kg4vOamIepgoK60EqnMgUs0mq0mS3rfJbNGERslwUeAME_3jbG9ZcPkxmCsfiT2HpHHDaq3S2IJqba07qxxABQs_0nJeksKK5AlDhtIHARwKo6FhFuNP6sdM_0G00__y30000)

  




### *Sequence*

```plantuml
@startuml
title Handling Prompt Transformations Sequence Diagram

actor "Prompt Gateway" as A
boundary "Plugin RESTful endpoints" as B
control "Internal Rule Engine" as C

A -> C : Receives a prompt
activate C
C -> A : Determines the order of transformations
deactivate C
activate A
A -> B : Sends the prompt to the Plugin modules for transformation
activate B
B --> A : Returns transformed prompt
deactivate B
deactivate A
@enduml
```


![Sequence](https://www.plantuml.com/plantuml/dsvg/N951KW8n44NtESKlzbp0XSKCKEhE6hX0EsamLPdEc7ImE9iBZ-GLR0P4T9Uk_Fx__UJhutDHbBBM8JeD6XcF93u6sUCvfs5KR3D9sQKya8Oa1Hj-gomzOnLedsbmZdfD6REB_e6Kt-au0nKqxYLLyPIFTXthFWYwzMQxgn4iVan1j4p2rlL9DAU8sPCePw68hXhDMco99ytIkGRpUonnXuvx3WSk88nJx0aY72pQPCkJgZ7LYirgiFq2VMMax3aZxQ1_ApdFDzlNOtEEQyreOxHdZtCQD4tJfTEGlB4McEK_xr-trhMONxWwrfhjAQzQzZyrRb3Qsw5n2sEmN_e6003__mC0)

  




### *Activity*

```plantuml
@startuml
title Handling Prompt Transformations Activity Diagram

start
:Prompt Gateway receives a prompt;
:Prompt Gateway uses the internal rule engine to determine the order of transformations;
:Prompt Gateway sends the prompt to the Plugin modules for transformation;
stop
@enduml
```


![Activity](https://www.plantuml.com/plantuml/dsvg/P90n3i8m34NtdCBdW0Ka91WxS06BkCLIubIEKzIpCN0ahe1RM83H_j_lstxU7xjggDPo2iQM24ueCR4Cq6d9ey5PKMfVDADnaGhxY_74DiE1SL3C8Qo0iFduZsXqmncKBiGJLK0OLsNxPsdLPRiIi1YfO09jlW79m49W1I9vFw-5kuf6KYWzsFTA_-1A4j_aT_J2MgekDKT3Bj5pAZZYXxNrOyeOTZxl7ta1003__mC0)

  




### *State*

```plantuml
@startuml
title Handling Prompt Transformations State Diagram

state "Prompt Received" as A
state "Order Determined" as B
state "Prompt Transformed" as C

[*] --> A
A --> B : Determine Order
B --> C : Transform Prompt
C --> [*]

@enduml
```


![State](https://www.plantuml.com/plantuml/dsvg/N8yn3i8m34LtdyBgYDGBCA2swS060ZR4O15JHMhIoZ5SZe4ZSGMYA1I9oRBUzxFzVhwQg2HwzoEeqv5nIy6EBWoubydFYYUX46-JU58tXOX79MNi7Gr27Y3cjYhX0r_PFTXMI17RBzo9PS6UbSMxK6ZtDxhiATm0d5SNhEjD4hMvThZ-MJ1ReSl49B88okLWCaeIW8Q3JIz-0000__y30000)

  




### *Class*

```plantuml
@startuml
title Class Diagram for Handling Prompt Transformations

class PromptGateway {
  +receivePrompt(prompt: Prompt): void
  +sendPromptToPlugin(prompt: Prompt): void
}

class InternalRuleEngine {
  +determineTransformationOrder(prompt: Prompt): TransformationOrder
}

class PluginModule {
  +transformPrompt(prompt: Prompt): Prompt
}

class Prompt {
  -content: String
  -transformationOrder: TransformationOrder
  +getContent(): String
  +getTransformationOrder(): TransformationOrder
}

class TransformationOrder {
  -order: Array<String>
  +getOrder(): Array<String>
}

PromptGateway "1" -- "1" InternalRuleEngine: Uses
PromptGateway "1" -- "*" PluginModule: Sends prompt to
Prompt "1" -- "1" TransformationOrder: Has
@enduml
```


![Class](https://www.plantuml.com/plantuml/dsvg/X5BDJiCm3BxtAQoUDb0FN2k4XWO23eW9nWD4ZLT5IfCfSOScn9Dnu95u1TowWrPjnIcj_VtPoVVdrogI2bHNDY535c5jLOnmOrGPL0Kx7-1UEMsDAs4JVBKds0Rb8ZSgHSQxc2H5Iv7kdI9yKqTuJm3E0nPe3YYTnRuzyWwupE7WZMvW4PsMujPlR5qQDuFzE7azECBWb7skBTuw9g0OQkHwnR_3Z4z1OnXhJe3-B2J8ezTi8U9qWi_D8nyz2TbNGyuApv4Teryey2wR4etjfpEnTucq5eN5igVHrAT6_M-uYQR4z9BYEWHrl1IRgyxdLtdOPCtXpQSNAMHPUuolAOVNY766SPOEjivpyiE887i6yXsjxp0nICuFDYOhflAZ_W400F__0m00)

  

## Configuring the Prompt Gateway
Description:
*An Admin configures the Prompt Gateway with any number of Plugins. *

### *Actors:*
- Admin

### *Pre-conditions:*
- The Admin has access to the Prompt Gateway configuration.

### *Post-conditions:*
- The Prompt Gateway is configured with the selected Plugins.


### *Flow:*
- The Admin selects the Plugins to be used.- The Admin configures the Prompt Gateway with the selected Plugins.



### *Robustness*

```plantuml
@startuml
title Configuring the Prompt Gateway Robustness Diagram

actor "Admin" as A <<actor>>
boundary "Prompt Gateway Configuration Interface" as B <<boundary>>
control "Prompt Gateway Configuration" as C <<control>>

A -> B : Selects Plugins
B -> C : Configures Prompt Gateway with selected Plugins
@enduml
```


![Robustness](https://www.plantuml.com/plantuml/dsvg/V91D2i9034RtSuhGlHUGKleZYBkY9uXjMWUc6PbaA9xDXKVo2ex5kd3XDb_U2_cUzqSfYa7Zv8nQTGIDvy6ECLWUGIy4RV3JLM6FIZFUuEZFKPH917OMnu2JCTYf3v1L_MGv0nIe8C-NOL6Oiu_SOxX1zcDQ3w5Qpt1WfJ1WHukWJe8LJ8xEimRl_YiMi4dWPpjnfe9DaMHRE96ZJWLQ5qVBOkftqAHWLP3yVZfRlO0i7FLVi2JkKral0000__y30000)

  




### *Sequence*

```plantuml
@startuml
title Configuring the Prompt Gateway Sequence Diagram

actor "Admin" as A
entity "Prompt Gateway Configuration Interface" as B
control "Prompt Gateway Configuration" as C

A -> B : Selects Plugins
activate B
B -> C : Configures Prompt Gateway with selected Plugins
activate C
C --> B : Configuration Successful
deactivate C
B --> A : Configuration Successful
deactivate B
@enduml
```


![Sequence](https://www.plantuml.com/plantuml/dsvg/Z91DQWCn34RtEeMOVIwGHSdOWT9T82SG7ir4u95RMsx8sRheaNg5EaC3-O7G_NjFd_tpzRqfHjdg7C748gDFEifKi-Y4Tc3SvJI_6xwIyP5EkEUFoXeOjq9JfXc0WgMCNJ_CeXrImHvOM-k4tPrxIPD9KdnJupnIu4Lo499QJl6vjR0UeCVL2pfSjpsHWnNSnJg9blCQ-Mnc2xepunjpAN2vVyrHx81bIV3msF3WSVLtw7RyleR0fOmrmi1Nl5lu_f-yWmthqFx-5m00__y30000)

  




### *Activity*

```plantuml
@startuml
title Configuring the Prompt Gateway Activity Diagram

start
:Admin logs in;

if (Admin has access to the Prompt Gateway configuration) then (yes)
  :Admin selects the Plugins to be used;
  :Admin configures the Prompt Gateway with the selected Plugins;
  stop
else (no)
  :Access denied;
  stop
endif

@enduml
```


![Activity](https://www.plantuml.com/plantuml/dsvg/R90n3i8m34Ltdy9ZUmMwK874oXL2ugQMKWVLBbNFni2Hk0BGW5WwMVhwVi_oy_xOgxdHfY61iIN2GvQEupIoHBIUy3pcuMfuSaQpMx3rnZUs1O_iukW6W7KVTcqOM33bgCZI0727LSbwf-Yy9rMqlCNqNwKppb9_6eBLGbe3ufUhbCYRbkqqHPOLTY6Sb4BpB_vOf5kccQrVyyAZy8Dz26hv2fIKi99StENgGC95KYeIk0FOl-VxRIy0003__mC0)

  




### *State*

```plantuml
@startuml
title State Diagram for Configuring the Prompt Gateway

[*] --> NotConfigured : Admin has access to configuration
NotConfigured --> PluginsSelected : Admin selects Plugins
PluginsSelected --> Configured : Admin configures Prompt Gateway
Configured --> [*] : Prompt Gateway is configured

state NotConfigured {
  [*] --> [Admin has access to configuration] Idle
}

state PluginsSelected {
  [*] --> [Admin selects Plugins] Idle
}

state Configured {
  [*] --> [Prompt Gateway is configured] Idle
}

@enduml
```


![State](https://www.plantuml.com/plantuml/dsvg/X95DYW8n48NtTOgt7l02BWP5GTmCWajn4Jfb6qWdGweQ4V5aBZoILz0MWnhysIQllbTVSZeS1w8cQTjuKgUUCLUZZB4pTJ8DLZ5X5CFArMroeOQk6RCKct_5v9BRc2tHucU9NkyNVr5pb2tw6Dh61QoDm5GLYq0Zgbl0g8k1dYSwniotjGioPy-LFb3aUY3vdifSD_kYFDUnb5iNlPr3lyZ0oHrWYUJwCiyxxmZ8_eklmajChMVQPrPfyO9MY9U4TwjyChap1XpivUVF0000__y30000)

  




### *Class*

```plantuml
@startuml
title Configuring the Prompt Gateway Class Diagram

class Admin {
  +selectPlugins(): void
  +configurePromptGateway(): void
}

class Plugin {
  +transformPrompt(): void
}

class PromptGateway {
  +receivePrompt(): void
  +sendPrompt(): void
  +accessPlugin(): void
}

class PromptGatewayConfigurationInterface {
  +selectPlugins(): void
  +configurePromptGateway(): void
}

Admin -- PromptGatewayConfigurationInterface : uses >
PromptGatewayConfigurationInterface -- PromptGateway : configures >
PromptGateway -- Plugin : uses >
@enduml
```


![Class](https://www.plantuml.com/plantuml/dsvg/f99D2W8n38NtFKMMYdW15w8eY3kl43gPCT0VQJAAY2TpuP6yWkDK1r51mMNRzxwNF7evdIUYcBJspYYh8vZ5K79L9muLw9fWbQBVA2nGQOTxc3aKWJbZbT0ROzlZjF0Su6001aAEhAvSNN6GNdy4syZ5xS7UkPI1TrwdE3vOsPfXcZ186PFFdW_YPrRs9BB4Mtfnj9E5ulqMhIMH7Fg5_sW6bMDO1gLKegM_ryxb3OS_XOsW5X8Ocr_4hyp6tOtpncZLkVikPD9KrloCAm000F__0m00)

  

## Accessing Transformation Plugins
Description:
*The Prompt Gateway accesses the Transformation Plugins via their respective RESTful endpoint URLs. *

### *Actors:*
- Prompt Gateway

### *Pre-conditions:*
- The Prompt Gateway is configured with the Plugins.

### *Post-conditions:*
- The Prompt Gateway has accessed the Transformation Plugins.


### *Flow:*
- The Prompt Gateway sends a request to the Plugin's RESTful endpoint URL.- The Plugin receives the request and establishes a connection with the Prompt Gateway.



### *Robustness*

```plantuml
@startuml
title Accessing Transformation Plugins Robustness Diagram

actor "Prompt Gateway" as A <<actor>>
boundary "Plugin RESTful endpoints" as B <<boundary>>
control "Plugin Connection Manager" as C <<control>>

A -> B : Sends a request to the Plugin's RESTful endpoint URL
B -> C : Receives the request and establishes a connection with the Prompt Gateway
@enduml
```


![Robustness](https://www.plantuml.com/plantuml/dsvg/P911JiD034NtSmgh6rPS05LHqu3OW5IbvG3En4r6cdZAsA7gsLXm9Aw0ExGWn6eMyz_pjp_VFrQYc7MUejEWaQ3sdaG23x3BoB9FUK8DYM4Jvo6mG9kwMPGj1FS1XuoJS-WrPLXiSfeE2e-eTCJJ0b2WXkMo_5QLwzBCFUQJ1OiBseVjRZz78EuFAR1AGTQ6NAD6-SIQK_o1ciHClbHwHiQ1Sg4QeovHWrmDjvMPxc1hSW64JAypYO8cq94kszp8lmxmqZwvzPbkZ6x9KtWZASnLWTo3lTZ582ETvVwtqp7e-3tXpp7SoWRObRy0003__mC0)

  




### *Sequence*

```plantuml
@startuml
title Accessing Transformation Plugins Sequence Diagram

actor "Prompt Gateway" as A
boundary "Plugin RESTful endpoints" as B
control "Plugin Connection Manager" as C

A -> B : Sends a request to the Plugin's RESTful endpoint URL
activate B
B -> C : Receives the request
activate C
C --> B : Establishes a connection with the Prompt Gateway
deactivate C
B --> A : Connection established
deactivate B

@enduml
```


![Sequence](https://www.plantuml.com/plantuml/dsvg/P951Ri8m44NtFiM8JLTSe8kA94NTj18Az023CoIMd37rJ42SZGL7wXNgk16awjf_td-U-RxULg8OTUYzKQUUeB2MH1oti8l8SWYnHtM1OUE7rh70bhu6Oalmwh2Ds1k3LaE4nIQ6_gZmXafdl2m01Ggp3mCt62_fESz3lTxk3eC7ukOO7AlaO6biO8t1ttDLO2QRcp-GiQMOWvKn1InVe8JdPCAD048SZKH10sX7a-YJ_Ak2p_fzb7Mdf9Wgon5K9L1Dbjo998zFj4UmCXKifyfrsjRUE-beBBOFnxFJxhVzpnPCGpDEcJb5uin-HtTaCq-NngoIU3hC3m000F__0m00)

  




### *Activity*

```plantuml
@startuml
title Accessing Transformation Plugins Activity Diagram

start
:Prompt Gateway is configured with the Plugins;
:Prompt Gateway sends a request to the Plugin's RESTful endpoint URL;
:Plugin receives the request;
:Plugin establishes a connection with the Prompt Gateway;
stop

@enduml
```


![Activity](https://www.plantuml.com/plantuml/dsvg/P50x3i8m3Drp2c_SWIuWWLWOK2a7CC5jBQL9YPqYpiR0aRW2eG8Ln6Yzln-t-q8KewRE6MLr14jhIOHz0rL4BtM87Ie73pkN6lQI2Se3wnLMZ4t4pfZHmSntCNIzmWQLBdW55h31rzoaI2UuiBQWBNriYZ--a3y984Gw9n853L_ycK2vtbTrSf1fVM2lS2YtBvSHpofBF92CchV5XEO3ZuwbfLT2hkN9ZgEcMZzbYhmfzCOiSbX-pHC00F__0m00)

  




### *State*

```plantuml
@startuml
title Accessing Transformation Plugins State Diagram

state "Prompt Gateway Configured" as PGConfigured
state "Request Sent to Plugin" as RequestSent
state "Connection Established with Plugin" as ConnectionEstablished

[*] --> PGConfigured
PGConfigured --> RequestSent : Sends a request to the Plugin's RESTful endpoint URL
RequestSent --> ConnectionEstablished : Plugin receives the request and establishes a connection
ConnectionEstablished --> [*]

@enduml
```


![State](https://www.plantuml.com/plantuml/dsvg/R971JiCm38RlUGfh5oIalG27QGYcNJXKwtP274Bhjf7IPCJE9fwD0u_4A-24TWlIZhP_V_wY_7nzhubru3XQnOOjmcFJ895nFUo3TjJvC6esta5bOsySGSsQ4PwDxeCUbQ9SBghWnoF3Legp_eGdxphJnu3j0ZH1jRqsveqTVaGaXXeT0_k9d-FJA4tcjEmxRBB8Hbhlrj20BPmD3-NcDLQab7gzVuFbSlNVeopoj3WB3ych9T0G9azHv06dOtSYkQdtNRGWiQCtidBOlQWIaP0tVGJ-Hn5sW-Q4bCdp8UrQm4iwAJGNYheDJAVaZKgjnKR-yXS00F__0m00)

  




### *Class*

```plantuml
@startuml
title Class Diagram for Accessing Transformation Plugins

class PromptGateway {
  -plugins: Plugin[]
  +sendRequest(plugin: Plugin): void
  +receiveResponse(plugin: Plugin): void
}

class Plugin {
  -endpointURL: String
  +receiveRequest(): void
  +establishConnection(): void
}

class PluginConnectionManager {
  -plugin: Plugin
  +manageConnection(): void
}

PromptGateway "1" -- "0..*" Plugin : sends request >
Plugin "1" -- "1" PluginConnectionManager : establishes connection >
@enduml
```


![Class](https://www.plantuml.com/plantuml/dsvg/T56zJiCm4Dxz52TF1PIAhXvGKP5OGAe2J8ZXS8zWARQ3pob2Y2TZu95u1Pou2QtKZBxly_Tpz_lzi-I88RQs5j74cc1L8pDS6Qm2MdZn0Iwr9cRZAhWFw3YDB4RZ7QphjZAEXT3zdtNmjedN6EaTF-1J01HDPgWTzV4f3S-OtAQajvOupZ9Xm4yKRBtPTAH0cioMIkB6EwO9ujVetO-pQP9ll77neRnHS1T3IdwWc9ttlD8Jdsl3holl7Eckssp2uPznYmuh2lizXtITfktXuuA7Yv8B2KK1ydm-FvL34GNTbXX2JWiNOWSCx8MSJAHWB4GCUiIJo3A9fbF_0G00__y30000)

  

