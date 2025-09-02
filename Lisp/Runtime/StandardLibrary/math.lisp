(def * Turbo.Multiply)
(def / Turbo.Divide)
(def + Turbo.Add)
(def - Turbo.SubtractOrNegate)
(def % Turbo.Modulo)

; increment
(def inc (lambda (a)
    (+ a 1)))
    
; decrement
(def dec (lambda (a)
    (- a 1)))

; power
(def pow (lambda (a)
    (* a a)))