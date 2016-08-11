package foundation.softwaredesign.orbi.persistence;

import javax.enterprise.inject.Disposes;
import javax.enterprise.inject.Produces;
import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.PersistenceUnit;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class EntityManagerProducer {
    @PersistenceUnit(unitName = "orbiDB")
    private EntityManagerFactory emf;

    @Produces
    public EntityManager create() {
        return emf.createEntityManager();
    }

    public void close(@Disposes EntityManager em) {
        if (em.isOpen()) {
            em.close();
        }
    }
}
