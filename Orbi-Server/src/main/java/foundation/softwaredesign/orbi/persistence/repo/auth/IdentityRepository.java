package foundation.softwaredesign.orbi.persistence.repo.auth;

import foundation.softwaredesign.orbi.persistence.entity.IdentityEntity;
import org.apache.deltaspike.data.api.AbstractEntityRepository;
import org.apache.deltaspike.data.api.Query;
import org.apache.deltaspike.data.api.Repository;
import org.apache.deltaspike.data.api.SingleResultType;

import javax.validation.constraints.NotNull;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@Repository
public abstract class IdentityRepository extends AbstractEntityRepository<IdentityEntity, Long> {

    @Query(singleResult = SingleResultType.OPTIONAL)
    public abstract IdentityEntity findByEmail(@NotNull String email);

    public abstract IdentityEntity findByToken(@NotNull String token);


}
