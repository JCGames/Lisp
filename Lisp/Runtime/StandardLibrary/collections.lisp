(def get Turbo.Get)
(def len Turbo.Length)
(def first Turbo.First)
(def rest Turbo.Rest)
(def apply Turbo.Apply)

(def empty? (lambda (x)
    (= (len x) 0)))
