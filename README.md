graph TD
    %% Pradžia
    start(A) --> register[Užsakymų priėmimas ir klientų registravimas];

    %% Sprendimo Taškas 1: Klientų duomenys
    register --> data_check{Ar reikia naujų duomenų apie klientus?};

    data_check -- Taip --> update_data[Atnaujinami duomenys];
    update_data --> planning[Maršrutų planavimas];

    data_check -- Ne --> planning;

    %% Sprendimo Taškas 2: Maršruto optimalumas
    planning --> optimal_check{Ar maršrutas optimalus?};

    optimal_check -- Ne --> optimize[Optimizuoti maršrutą];
    optimize --> planning; %% Grįžimas atgal planavimui

    optimal_check -- Taip --> execution[Maršrutų vykdymas];

    %% Sprendimo Taškas 3: Nukrypimai
    execution --> incident_check{Ar įvyko nukrypimų / incidentų?};

    incident_check -- Taip --> correction[Koreguoti planą, informuoti dispečerį];
    correction --> execution; %% Grįžimas atgal vykdymui

    incident_check -- Ne --> continue_exec[Tęsti];

    %% Pabaigos žingsniai
    continue_exec --> confirm[Surinkimo užduočių patvirtinimas];
    confirm --> report[Ataskaitų rengimas ir analizė];

    %% Pabaiga
    report --> end(B);

    %% Stilių apibrėžimas
    classDef startEnd fill:#333,stroke:#333,color:#fff;
    class start,end startEnd;
    classDef decision fill:#cce,stroke:#333;
    class data_check,optimal_check,incident_check decision;
    classDef activity fill:#fff,stroke:#333;
    class register,update_data,planning,optimize,execution,correction,continue_exec,confirm,report activity;
    
    linkStyle 0 stroke-width:2px,fill:none,stroke:#000;
    linkStyle 1 stroke-width:2px,fill:none,stroke:#000;
    linkStyle 2 stroke-width:2px,fill:none,stroke:green;
    linkStyle 3 stroke-width:2px,fill:none,stroke:green;
    linkStyle 4 stroke-width:2px,fill:none,stroke:#000;
    linkStyle 5 stroke-width:2px,fill:none,stroke:red;
    linkStyle 6 stroke-width:2px,fill:none,stroke:red;
    linkStyle 7 stroke-width:2px,fill:none,stroke:green;
    linkStyle 8 stroke-width:2px,fill:none,stroke:#000;
    linkStyle 9 stroke-width:2px,fill:none,stroke:red;
    linkStyle 10 stroke-width:2px,fill:none,stroke:red;
    linkStyle 11 stroke-width:2px,fill:none,stroke:green;
    linkStyle 12 stroke-width:2px,fill:none,stroke:#000;
    linkStyle 13 stroke-width:2px,fill:none,stroke:#000;
    linkStyle 14 stroke-width:2px,fill:none,stroke:#000;
