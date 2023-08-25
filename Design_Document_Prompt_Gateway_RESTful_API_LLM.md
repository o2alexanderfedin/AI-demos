# Prompt Gateway for External LLM with RESTful API and Plugin Support
## *Designing a Prompt Gateway with RESTful API Integration, Plugin-based Prompt Transformations, and Admin Configuration for an External LLM*

## *Abstract*
*The design document for the Prompt Gateway task outlines the creation of a RESTful API-accessible Prompt Gateway for an external LLM. The Prompt Gateway is responsible for reliably accepting prompts, applying optional transformations, and sending the transformed prompts to the LLM. To achieve this, the Prompt Gateway utilizes a combination of internal and external Plugin modules for prompt transformations. The order of these transformations is determined by an internal rule engine. The Prompt Gateway can be configured by an Admin with any number of Plugins, which are accessible via their respective RESTful endpoint URLs.*
***In addition to the Prompt Gateway implementation, the design document also specifies the need for a local startup shell script. This script will be used to initiate the Prompt Gateway and ensure its proper functioning. Furthermore, deployment artifacts for Ansible and Docker are required to facilitate easy deployment and management of the Prompt Gateway in different environments.***
*To ensure seamless usage and configuration of the Prompt Gateway, comprehensive documentation needs to be produced. This documentation should cover various aspects such as usage instructions, plugin configuration guidelines, and troubleshooting techniques. By providing detailed documentation, users and administrators will have a clear understanding of how to interact with and maintain the Prompt Gateway effectively.*

## *Task*
```Task
Create a Prompt Gateway for an external LLM with these specifications:
- Prompt Gateway is accessible to clients via a specific RESTful API URL.
- Prompt gateway reliably accepts prompts, then applies optional transformations, and sends transformed prompts to the LLM.
- Prompt Gateway handles Prompt Transformations via any number of external and/or internal Plugin modules.
- Prompt Gateway uses an internal rule engine to determine the order of the Prompt Transformations by the Plugins.
- An Admin can configure the Prompt Gateway with any number of Plugins.
- Prompt Transformation Plugins are accessible to the Prompt Gateway only via their respective RESTful endpoint URLs.

Provide a local startup shell script.
Create deployment artifacts for Ansible and Docker.
Produce comprehensive documentation: usage, plugin configuration, and troubleshooting.
```

## *Design*


### *Use Cases*

```plantuml
@startuml
left to right direction
actor "API Caller" as API
actor "Admin" as Admin
rectangle "Sending Prompts to the Gateway" as SPG {
  (Sending Prompts to the Gateway) as SPG1
  API -- SPG1
}
rectangle "Handling Prompt Transformations" as HPT {
  (Handling Prompt Transformations) as HPT1
  SPG1 <.. HPT1 : <<uses>>
}
rectangle "Configuring the Prompt Gateway" as CPG {
  (Configuring the Prompt Gateway) as CPG1
  Admin -- CPG1
}
rectangle "Accessing Transformation Plugins" as ATP {
  (Accessing Transformation Plugins) as ATP1
  CPG1 <.. ATP1 : <<uses>>
}
@enduml
```


![Use Cases](https://www.plantuml.com/plantuml/svg/X991ReCm44Ntd6B4AYmIATiAHK5U9DrP2XSOWN4i6Rko1qLLr9DrqIFr2cN0QOCg9Ji6dl-V1tn_VkqZOQ2XjvbL7G5v24QV2LeJL4F6kmmRyW7oIhw2G6jLo04ZZEFlaxOtRhx9LnaVHAUjWloaN6kS1Xby_qQHu-ciu82aBlW-dJd90rmpWDLZjaZiTaHvbVLwdZxkwuxeMlkN0NL05piVUcIJEFKTPJttFO6B17CXDq6vsKmpl41P3b75tMxPBhphZ1u2P_BcAVTULjna7xD5OYTPVgUiA_xfLasZOkI8vUuWxQ1DygrgELS-euj4Swcu2VEy5DwFdsdyNNu0003__mC0)

  



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

- The API Caller sends a prompt to the Prompt Gateway.
- The Prompt Gateway applies optional transformations to the prompt.
- The transformed prompt is sent to the LLM.



### *Robustness*

```plantuml
@startuml
title Sending Prompts to the Gateway Robustness Diagram

actor "API Caller" as A <<actor>>
boundary "Prompt Gateway API" as B <<boundary>>
control "Prompt Gateway" as C <<control>>
entity "Prompts" as D <<entity>>

A -> B : Sends a prompt
B -> C : Delegates prompt processing
C -> D : Transforms and sends prompt to LLM
@enduml
```


![Robustness](https://www.plantuml.com/plantuml/svg/ND1HQiCm30RWTvz2vBqN62KqJM0CMYZjBg0cMXPmx68fb9vj3plIhh0_Rjg7FXcapxyj_VtyRjGusJHwPuDvePE4RWWz7L8SBwPaaUnRw9rDhdoZOpnFQa5KgHcuJpmwnwt5H4Lr-A2QlPTK42jLj5xdJbcwSvn2n-b6nJts6OSx6M-17mY-ZS5IzAyyqnfqwKDAmBUVkPf50t6l0xYALYNot_9aIaoNJDrshjUeD-AbnmEwTEQZnO3OWgjdr01z9Wxw5TE8XD2Hvgp5OqMxtTvjKCCU_m400F__0m00)

  




### *Sequence*

```plantuml
@startuml
title Sending Prompts to the Gateway Sequence Diagram

actor "API Caller" as A
boundary "Prompt Gateway API" as B
control "Prompt Gateway" as C
entity "Prompts" as D

A -> B : Sends a prompt
activate B
B -> C : Delegates prompt processing
activate C
C -> D : Transforms and sends prompt to LLM
activate D
D --> C : Acknowledges receipt
deactivate D
C --> B : Returns transformed prompt
deactivate C
B --> A : Returns transformed prompt
deactivate B

@enduml
```


![Sequence](https://www.plantuml.com/plantuml/svg/Z95DJWCn38NtFeKr-rw01MecaH1IaIhW0YOnmuXCqYOPgfwD1KVY2ZXX-YDMh2BPttj-Thu_lzQvK2hpvA6CnJC-SN1Z672RuhGh6Kl4yiPuHuKFT9JsVkPW6Sr8Gw89W6o92JkrlKTDtdFga38gU8bpS9IEsFrQdIs4R4WFDeQIelzBjAu63fBeBC-jQW0KhcwnnvkMD2FXhlLhZl533CIthuWMnB3dGMfvWUfZEMTPyC9hq9KtmZyd2larfabyWyFS9YnIESHcyt2H6J2uMkOe-nxYmRCRP5HYowC4Sdp5wiRMs8zSvXJaiATPx4uhN2bqNK8Kwj-A7c0jUUKdVm000F__0m00)

  




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


![Activity](https://www.plantuml.com/plantuml/svg/R911QiCm44NtEiLVEbU8DmaDb402WVO2WprE1R96QAODFbiMFLAlKDcEJGfTQlJ__3V_v_wzKwDidtrmvYqmtZYsFbvnoT8dKvZ0FXYlP7oZ0Vl6_9Ut0GTFrqozSvFVRVUN8rue1CxmYaPYvCQuNVppkcLDxPplK3rvjCGg26dM_UlQZBE8Qc3TE63xznUKKl2ia6HU8WLOfgYTv9x6mNhA756F6zuNHG5z91Vumh8Id4xdsd5GHXLbIds2RgN4HR69VeKl-b9XDpOZoUtAexJx2m00__y30000)

  




### *State*

```plantuml
@startuml
title State Diagram for Sending Prompts to the Gateway

state "API Caller Connected" as start
state "Prompt Sent to Gateway" as sent
state "Prompt Transformed" as transformed
state "Prompt Sent to LLM" as end

[*] --> start
start --> sent : API Caller sends a prompt
sent --> transformed : Prompt Gateway applies transformations
transformed --> end : Transformed prompt is sent to LLM
@enduml
```


![State](https://www.plantuml.com/plantuml/svg/T94nQWCn44LxdUApfM4lu28uhC4ODc1mkf1Yy2gEG2i9QOBnsLpaIBb2Hbe52qDA3U-_-OD-Rj-Rn1JvQxQA3LkD8nDhR0sT8ytuy157xIRZpZX4FmTEO0_-rDW9Tw6hKgaakkV37WDPgoC6xvm-iPuwK49Py4SjakpaBAgIXPFP0_OQoINfC5SLtz__2CVnfP3IMQctrJlw_kdU8FBopl0QJMEPJ0c4K6og09bi5agWxggbGI5OevjMnCQxfDfGbeXRmiqjTGtCSdJjhJO2oaVy0W00__y30000)

  




### *Class*

```plantuml
@startuml
title Class Diagram for Sending Prompts to the Gateway

class APICaller {
}

class PromptGatewayAPI {
  +sendPrompt(prompt: Prompt): void
}

class PromptGateway {
  +processPrompt(prompt: Prompt): void
  +transformPrompt(prompt: Prompt): Prompt
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

APICaller -- PromptGatewayAPI : Sends prompt >
PromptGatewayAPI -- PromptGateway : Delegates prompt processing >
PromptGateway -- Prompt : Transforms >
PromptGateway -- LLM : Sends transformed prompt >

@enduml
```


![Class](https://www.plantuml.com/plantuml/svg/X99BQiCm48RtEiKi4u8lu4L9SQ0KKWWa5n3sr1N8Sj1CKqBfoRheaNg5ZYt7JgMuNLcU-RzvVtz-hOcrvzV6AZPi4GghYM1ZTErr0y-jXpswohWQThvjZap0BV0BmeDcVDDdfSgUkDyz5jfQzF2kFYx6W0nAKOWFO4KIC7WMn_wJ3y9b3gVML3DyW8KeaUXFNcJijIEflfaJXaUSgi-HbQrZTABUivVEEzCO3wiYyQvgvC6wM4RsGmB-bj24Hcclkg6RSUPhIWQvtJw5yNaiqPpml_5FsyooT8jvVnC40OSxbIXYIf0DMgpbRyI6JNQd5KMOSE76nkcMh6liKixLFARIr5fySju_0000__y30000)

  

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

- The Prompt Gateway receives a prompt.
- The Prompt Gateway uses the rule engine to determine the order of transformations.
- The Prompt Gateway sends the prompt to the Plugin modules for transformation.



### *Robustness*

```plantuml
@startuml
title Handling Prompt Transformations Robustness Diagram

actor "Prompt Gateway" as A <<actor>>
boundary "Plugin Modules" as B <<boundary>>
control "Rule Engine" as C <<control>>
entity "Prompts" as D <<entity>>
entity "Transformation Rules" as E <<entity>>

A -> B : Sends a prompt to Plugin Modules
A -> C : Uses Rule Engine
C -> D : Determines the order of transformations
C -> E : Determines the order of transformations
B -> D : Transforms the prompt
@enduml
```


![Robustness](https://www.plantuml.com/plantuml/svg/bD71ReCm30RWUvx2ujuNc2hK5j1sgJInxG7SuZAa45UnKSMpxR17sYlC8wM2uwxnv_XtylFrlHF5g4FldNRg2LumDBuBBRn6xgyAxn63N3Zsg1q7WPfFWsWW4IWxR2Fspk5PEK9sxtX6fHkE6Q307hRRgPhdxiH3Q32E1ltGTW6Et0oUP887WpCmU-QWaJrajGceWd6QN67kNZH6mKAFy-HqKsaYdI_0UWke7tEhfNPxsEGMvGdUA3G22DUqaZAiCoTPcFmG4bYaTCLVfRHAIKgnjoC1_IJWs504le2kNpGrLFzeECmJ7bkbXXJMxIox_USl0000__y30000)

  




### *Sequence*

```plantuml
@startuml
title Handling Prompt Transformations Sequence Diagram

actor "Prompt Gateway" as A
boundary "Plugin Modules" as B
control "Rule Engine" as C
entity "Prompts" as D
entity "Transformation Rules" as E

A -> D : Receives a prompt
A -> C : Uses Rule Engine
C -> D : Determines the order of transformations
C -> E : Determines the order of transformations
A -> B : Sends the prompt to Plugin Modules
B -> D : Transforms the prompt
D -> A : Returns transformed prompt
@enduml
```


![Sequence](https://www.plantuml.com/plantuml/svg/b54xJiGm4ErzYgVqN802RNw21ccLXGEOU3POSknbF0RbR1GSYIjW4X9jIjhtcVTcvllpwn15iwJHAR7Y20_Q6sVzW2E7yI9uOktZEV2enGOVSQAtHBudD5OFh4UbT2-1KVpfxxNGXvuAw8XIlOJaZUOfqouDrkCncEGepdIb-k25WqFHPH2jpmgQgLgHptscTUnYQ3PqtmhTDhDLgiJD7HhSegEUx3j5Q5pcAGjLP-evPlWgLDMhgI4X7ZCK8Q-4m8OOuGpPts4njFympDbLDfp8cqMxj884xA-ZghNDjkUrGJM_T3blA8dpKxOiCglca5FoLty0003__mC0)

  




### *Activity*

```plantuml
@startuml
title Handling Prompt Transformations Activity Diagram

start
:Prompt Gateway receives a prompt;

:Prompt Gateway uses the rule engine to determine the order of transformations;

:Prompt Gateway sends the prompt to the Plugin modules for transformation;

stop
@enduml
```


![Activity](https://www.plantuml.com/plantuml/svg/RD0x3i8m30RWFQVmEUXI92GOEt01gt6BfJmgnodYR0mSYIlWjYmyHkktlZ_ny_veYw9eZS4fQo0uOVA1qmYTv3Wfd0LJ6R94LCwfmBvNdbblS60S1QDpA-1sx_qZAbtn1a8zyKm54AOrQTpFJYsMwuL0gZLJ6ZaHQ0PFIXBNmS8idWJo0Fhvb3zWeUGtSUjSi6NgGZKROlPMLC2CBwnPlf4drnfWjtW1003__mC0)

  




### *State*

```plantuml
@startuml
title State Diagram for Handling Prompt Transformations

state "Prompt Received" as A
state "Determining Transformation Order" as B
state "Prompt Transformation" as C
state "Prompt Transformed" as D

[*] --> A
A --> B : Use Rule Engine
B --> C : Send Prompt to Plugin Modules
C --> D : Transform Prompt
D --> [*]

@enduml
```


![State](https://www.plantuml.com/plantuml/svg/T91D2i8m44RtSuh1fU05N4Xj8xWHYpyhSH7CM09D8idKoxdmI5x1CRH21LUpU6--ZvdNuraarf4xrm9RRWdth9bGMTr4xV0Q8gwrDwtr3TOnk1lZ8MgV13ZDDlW4aFBAfCSxkf2zavcWJhWSe2AcwAp_v8m3S1iDnMoNFr5ZCIlLFwKlL02dwHbdiuLqB_CiSOx7HBZhvBoLRwmdA3EfXEp9c-4o3bYtdGYu2KRi15Nsb7ZVebu6bP6K0HII8Hzy0m00__y30000)

  




### *Class*

```plantuml
@startuml
title Class Diagram for Handling Prompt Transformations

class PromptGateway {
  +receivePrompt(prompt: Prompt): void
  +sendPromptToPlugin(prompt: Prompt): void
  +useRuleEngine(): void
}

class PluginModule {
  +transformPrompt(prompt: Prompt): Prompt
}

class RuleEngine {
  +determineOrderOfTransformations(prompts: Prompts, rules: TransformationRules): void
}

class Prompt {
  -content: String
  +getContent(): String
  +setContent(content: String): void
}

class Prompts {
  -prompts: List<Prompt>
  +getPrompts(): List<Prompt>
  +setPrompts(prompts: List<Prompt>): void
}

class TransformationRule {
  -order: Integer
  +getOrder(): Integer
  +setOrder(order: Integer): void
}

class TransformationRules {
  -rules: List<TransformationRule>
  +getRules(): List<TransformationRule>
  +setRules(rules: List<TransformationRule>): void
}

PromptGateway "1" -- "1..*" PluginModule : sends prompt to >
PromptGateway "1" -- "1" RuleEngine : uses >
RuleEngine "1" -- "1" Prompts : determines order of transformations >
RuleEngine "1" -- "1" TransformationRules : determines order of transformations >
PluginModule "1..*" -- "1" Prompts : transforms >

@enduml
```


![Class](https://www.plantuml.com/plantuml/svg/Z5HBQiCm4Dtx58DNwIU1Raq9WRIqXPQ4DXSGySG8R2benYcbz6HTz4YzGYLB_iKnpShMp9ltten7_lt-MH0BmsMHHomvHrZcWWZkfCYCA62d3JmAbUPIPR0skjWpR8nGP1E5OAaLHT6sAl7P1y7uBZxWCmAuChX5UK2VcUohHnA05maSj4mTZ52bFhZHwxpCf1h7beIlPOxtog9mKYU-6XDL-OjEBSPxuDhjc0l_qbAqz9uWHKPJsEFAf6XMkwFs0o7LH7GDnZBOSn_eQ6deru_K2KstMZ4gQ-sDZHstqywGbpxgMctZrCQFgaOKo4iqNfybyQrFpODG03gXuoorsPCC0z5XwrvVkn4cy6GjPsY2S3LN9zi9KntkLvmX53eDtw1oEGJLFLS5JSSZEAfn_t1spFNlGdmJmtHg7xFPPTnVqGJSyXFuiG9hc8yLnztDJC1U1BBWJgo3h3zw0its4bIZ1Bq3xc_mACkfuPxBsEioT3umrvIvYcXX9s7_Gty0003__mC0)

  

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

- The Admin selects the Plugins to be used.
- The Admin configures the Prompt Gateway with the selected Plugins.



### *Robustness*

```plantuml
@startuml
title Configuring the Prompt Gateway Robustness Diagram

actor "Admin" as A <<actor>>
boundary "Prompt Gateway Configuration Interface" as B <<boundary>>
control "Prompt Gateway" as C <<control>>
entity "Plugin Modules" as D <<entity>>
entity "Configuration Settings" as E <<entity>>

A -> B : Selects the Plugins to be used
B -> C : Delegates the selection of Plugins
C -> D : Retrieves the selected Plugins
C -> E : Updates the Configuration Settings with the selected Plugins
@enduml
```


![Robustness](https://www.plantuml.com/plantuml/svg/RD5HQiCm30RWTvz2v7qN62KqJShOGs5qx01EhAG6nogslDApxM57sXMcECjeIj-C_UcNRFzyVMyZwy1fT8ejEuIQV6-75AmVW4y8Ru76CyEBPhpeAnofJP4znWYDrKFGerAwOmfGL6QqlW0TeOBD9c-MfMefUQF35OgxfAMHPaiUNZrZw7M7EM0d0KkXP7JaEP2xZyYq5lftBXAzt6BgvT9WFHp898SnmqRWV7pZraEy8xFSU_RxMwygU2fbh6Sn3ZkEyzlaBh8cQ15IHADsawl5DU86cNEMCLTDFQXVobGzsKRi4JbO_5nPD6kt5_TnDl-9ZoU7Y-NJuvGjUYFV_0i00F__0m00)

  




### *Sequence*

```plantuml
@startuml
title Configuring the Prompt Gateway Sequence Diagram

actor "Admin" as A
boundary "Prompt Gateway Configuration Interface" as B
control "Prompt Gateway" as C
entity "Plugin Modules" as D
entity "Configuration Settings" as E

A -> B : Selects the Plugins to be used
activate B
B -> C : Delegates the selection of Plugins
activate C
C -> D : Retrieves the selected Plugins
activate D
D --> C : Returns selected Plugins
deactivate D
C -> E : Updates the Configuration Settings with the selected Plugins
activate E
E --> C : Confirms update
deactivate E
C --> B : Confirms Configuration
deactivate C
B --> A : Confirms Configuration
deactivate B

@enduml
```


![Sequence](https://www.plantuml.com/plantuml/svg/Z99DRW8n38NtFeN5dWjqKJ5_ghgeX8fwW30nGwIPX2OEYBDrqIFr2dK2J0UerEwY-7tlFKVv-lXS15QUuz036-u9AsStfelUs0vvIxZqRjWnFYccWphYYjuZsPQmDghpQW1GBJkFiq8FnivG1InWxQBLoXznTaDVp1KRP_7PClcDQYbZ9RJEidVzBPMd5P2LWicoZvsn-E9qx2daOJqEhmDMn2nxd4GDG87pHopnGGOzjHnE2sO_EJjS4yP0EgrazX8kdSg4L8BKWdHoTu92DaWPRdDn-C4gg19M2_PAx0tjhp3IzqWDDSxFKS94Butkr9ec-XpHYFnjfyTUl--F1yFRFmeqq8m5ieiV0iPiFSrjKkxv5KVPLUfKNQNt4tNnBtK9i22hvIz-0m00__y30000)

  




### *Activity*

```plantuml
@startuml
title Configuring the Prompt Gateway Activity Diagram

start
:Admin logs in;

if (Admin has access to the Prompt Gateway configuration?) then (yes)
  :Admin selects the Plugins to be used;
  :Admin configures the Prompt Gateway with the selected Plugins;
  :Prompt Gateway retrieves the selected Plugins;
  :Prompt Gateway updates the Configuration Settings with the selected Plugins;
  stop
else (no)
  :Access denied;
  stop
endif

@enduml
```


![Activity](https://www.plantuml.com/plantuml/svg/Z951QWCn34NtEeMMaoiqYoQaq6sX9t1iZKVWaGTBJfYpTT4ZzGgTZYUqb52waX3_F_dB7s_l1pLNh8u9Z2qHdhBq76jXYMW3uKl9usJux8okRiQZDxwmpVZ4BXOt0gn-U3Y6aGLJZeeiUm3kiMkpmIawxqaLBTzZ-csbCyxokFkI27OpwGuGDx1I8c_QxAb6bfLs9gnAOVyjlC58xwswiWthlF4et60huPUua1McoqRwZwDEOUcQ_lGp5RwIsN9I_Vi5Qda2IahOIMxfs-422RUOJIA1Uu33KfUV-mG00F__0m00)

  




### *State*

```plantuml
@startuml
title State Diagram for Configuring the Prompt Gateway

[*] --> PluginSelection
state PluginSelection {
  [*] --> NotSelected : [Admin has not selected any Plugins]
  NotSelected --> Selected : Admin selects Plugins
  Selected --> NotSelected : Admin deselects Plugins
  Selected --> PluginConfiguration : Admin confirms selection
}
state PluginConfiguration {
  [*] --> NotConfigured : [Prompt Gateway is not configured with selected Plugins]
  NotConfigured --> Configured : Admin configures Prompt Gateway with selected Plugins
  Configured --> NotConfigured : Admin removes selected Plugins from configuration
  Configured --> [*] : Admin confirms configuration
}
@enduml
```


![State](https://www.plantuml.com/plantuml/svg/X991QiCm44NtEiLV5tA1BafA2zsLG9OX2s6FRO4b5MdS44fEraMFr2jKigB6aWlTpinxe_dsz-VNrhDqFEbHiEAHi6V9X6SbUoSrEklmP4sd-iafquC7mivP_SPu2NCdUHRYS7V4PlE0tJZroknff8QLDSBFfggA3m5aujLoQb2BUnmUMwqC1kbXBCFdZZJdgyKV0xo4ecHXI884-im4f9WlboQafN-WLCv9oFcI33UnwhI_BewdNuhZIwmA83TJ16MsK2cAvZPpKZpSeYbZMQYYl30ldXfBllgAVtk3jdBM3qvQHzg-ar_nwCAItwNp-Mjbp68LPSbSn9PC6_xH7m000F__0m00)

  




### *Class*

```plantuml
@startuml
title Class Diagram for Configuring the Prompt Gateway

class Admin {
}

class Plugin {
  +selectPlugin()
}

class PromptGateway {
  +configure(Plugin)
}

class ConfigurationSettings {
  +updateSettings(Plugin)
}

Admin -- PromptGateway : Configures >
PromptGateway -- Plugin : Uses >
PromptGateway -- ConfigurationSettings : Updates >
@enduml
```


![Class](https://www.plantuml.com/plantuml/svg/R91D2i8m48NtESKiAz8BT26L2cuBuW52Eus1_9HaWeXuCXSUoIlO9grKwVRbUsy-ydw-Ia_SKT2AaII5K2dkFHma5uvhwAo3offEYk2a4K0NXDfPtHCSEU6LtnXhOcFVQcdWpXvpKAiWOWAmzQYmePHacmKJfwQbX3RJDSmIlg1d4KxIcXCI3KO-jKBV3YDpjgmchpp_krLyrz33Zlq-ZdIoB-3iru5rcO6F9cEbHDCE__e1003__mC0)

  

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

- The Prompt Gateway sends a request to the Plugin's RESTful endpoint URL.
- The Plugin receives the request and establishes a connection with the Prompt Gateway.



### *Robustness*

```plantuml
@startuml
title Accessing Transformation Plugins Robustness Diagram

actor "Prompt Gateway" as A <<actor>>
boundary "RESTful endpoint URLs of Transformation Plugins" as B <<boundary>>
control "Prompt Gateway" as C <<control>>
entity "Plugin Modules" as D <<entity>>

A -> B : Sends a request to the Plugin's RESTful endpoint URL
B -> C : Receives the request and establishes a connection
C -> D : Accesses the Transformation Plugins
@enduml
```


![Robustness](https://www.plantuml.com/plantuml/svg/T94zRiCm38LtdOBmqgaNA08ZZbrqQO70IGz0sxGZG9PIaMgHitNeaNg5qZ_Pt4a3t_lU8_Nx_RDdmIXfT4QiE8AYROdP-W7E4Jtt8OueDdWukZHOpr279h5u5K5bSOWu6eEjX0ZPCORnAl22Gbzuom0P2jZjvccUcoOats6yGLO_dyvzSa2-kmRh1TxhLuRG_vCvamvAkXCKrWOlCRZDq5AbwroLv7Kp3LrWy1Qwv6XXLYfSngep1Jpc6lC49-t5W13f8n4BI02vqDhcGK-mKTyS9dUfxffQif_4i-UEGD-1ljWuonUQu5hGKpkjQSh9MgbrEVvgtRw5sMkgVjSV0000__y30000)

  




### *Sequence*

```plantuml
@startuml
title Accessing Transformation Plugins Sequence Diagram

actor "Prompt Gateway" as A
boundary "RESTful endpoint URLs of Transformation Plugins" as B
control "Prompt Gateway" as C
entity "Plugin Modules" as D

A -> B : Sends a request to the Plugin's RESTful endpoint URL
activate B
B -> C : Receives the request and establishes a connection
activate C
C -> D : Accesses the Transformation Plugins
activate D
D --> C : Connection established
deactivate D
C --> B : Connection established
deactivate C
B --> A : Connection established
deactivate B

@enduml
```


![Sequence](https://www.plantuml.com/plantuml/svg/Z98nRW8n44NxESM89XKNI16nwutI10a1ES1W3s39EoRsB15dIv4ZvGgnWGGA8b6v-V_z_qV-_loQPAMaGnz0lGR6nbhEsSi6buaahsFgINqKd8LXuoNZWjy75ilOUTeawW78QamucgNOxnHVIFc33YEaZ0siuY2EqW57y-V5SZq4P76xw4NnRVwQCQx_oJdwMx1HDCLmasw0fPGkx9C7fz4DWK_M3g31yHEs-5WgYyj8c6hrhAWHTSldf8UCjwhLLNvVqag9je9C0StPijzpFjf_QIGEoqkhuFEMQqufBMphcWl5WAcKhb1E9pvJRg-_-3heS7oEDt_Sgq07Zg_KvgXkxrARkgoecxlKBS2aNAXybHy0003__mC0)

  




### *Activity*

```plantuml
@startuml
title Accessing Transformation Plugins Activity Diagram

start
:Prompt Gateway sends a request to the Plugin's RESTful endpoint URL;
:Plugin receives the request and establishes a connection with the Prompt Gateway;
stop

@enduml
```


![Activity](https://www.plantuml.com/plantuml/svg/L8yn3i9030JxUuKx_a0Qa40q52Y41vZ3IInTVE7i9E9j53o9Bt04AAXSU7Tsztb_hDGmsT05Pso1OEqzgR8qK2KKhMFgq3WA7CBGi6WM68ziDzWmDWaxvsQ2MnvIx7g37HfDU0CbkIWW9BeEf0OMmLhwOXOAvVPOrKE0BEiZYy6ft1UPCl-ponEFfBFdXq2vGBvu3gmjlU4-Yf2V-qrixIVXhqUHwyNUkLNEoIjV0000__y30000)

  




### *State*

```plantuml
@startuml
title Accessing Transformation Plugins State Diagram

state "Prompt Gateway Configured" as PGC
state "Request Sent to Plugin's RESTful endpoint URL" as RS
state "Connection Established with Plugin" as CE
state "Accessed Transformation Plugins" as ATP

PGC --> RS : Sends a request
RS --> CE : Receives the request
CE --> ATP : Accesses the Transformation Plugins

@enduml
```


![State](https://www.plantuml.com/plantuml/svg/R50xRiCm3Drr2Y9BfroWGv70DR9qC6Jt0AfDsmHiAH6f1ZrRXnwfLoWjsvqoakzB_dx_d5XSb3HFHaWcX7FR8ZFv0PheFFSXpaueUAYcD91dgCK9mYkv8RhP6CxZeOfXlWfSTBYxBoY2xsb84Ri3E8RgKkn4YxU4B52Z5v2msJunsB9k-ZG1-kuQIC5t-vR5jjwrwkgnpMrANNrCn2DsS2SPDvyiACfTi1wZbCUtPFQvgOpHWd0ylcWMF2_LEWO7SQrgTBbWHQcOnHRf4nban7-28Wj1dPInPQwCnxd6d3H2V_u7003__mC0)

  




### *Class*

```plantuml
@startuml
title Accessing Transformation Plugins Class Diagram

class PromptGateway {
  -configuredPlugins: Plugin[]
  +sendRequest(plugin: Plugin): void
  +establishConnection(plugin: Plugin): void
}

class Plugin {
  -endpointURL: String
  +receiveRequest(): void
  +establishConnection(): void
}

class RESTfulEndpointURL {
  -url: String
}

PromptGateway "1" -- "0..*" Plugin : accesses
Plugin "1" -- "1" RESTfulEndpointURL : has
@enduml
```


![Class](https://www.plantuml.com/plantuml/svg/X96_JiCm4CPtFyKf4t-K2jPCHQLYOQZICY66mxcabXmx-Cv52F5a33mIhu2J9aM88YRBzts-ttVdp_UFEGi6YQrLOiGIN6bDpCOri0dekFQXHJ7UmSh6nZY6XKLck3RO16oLqiDr5NpRoGqAFUCBl2g0N7jNcoO6giQNnTZY_Y79vqokAkaf4ij9DzGd_RI0dJTLRqeYFbh3suLtZdIVum_pso79KDv7I8ZE6oTtvMq1Qmbfghvh84rcHnFz7zulGhbSR-febz_DzxGOx86Ip3zNabrcaEUGNSncPzaKiW0SjasinifaIySHJ05RP3LFOwN_-W800F__0m00)
